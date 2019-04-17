using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using personal_site.Models;
using personal_site.Helpers;
using personal_site.ViewModels;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Specialized;
using LinqToTwitter;
using personal_site.Services;

namespace personal_site.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                _userManager = value;
            }
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLogin(string provider)
        {
            ControllerContext.HttpContext.Session.RemoveAll();
            //Handle external twitter logins separately
            if (provider == "Twitter")
                return await TwitterExternalLogin();

            // Request a redirect to the external login provider
            else return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account"));
        }

        [AllowAnonymous]
        public JsonResult ExternalLoginMyCallback()
        {
            return ControllerHelper.JsonActionResponse(true, "testing!");
        }


        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<JsonResult> ExternalLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            var email = loginInfo.Email;
            Debug.WriteLine("Default user name: " + loginInfo.DefaultUserName + " EMAIL: " + email);


            if (loginInfo == null)
                return ControllerHelper.JsonActionResponse(true, "You need to login");
            

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return ControllerHelper.JsonActionResponse(true, "Successfully signed in");
                case SignInStatus.LockedOut:
                    return ControllerHelper.JsonActionResponse(false, "Locked out");
                case SignInStatus.RequiresVerification:
                    return await SendCode(false);
                case SignInStatus.Failure:
                default:
                    AccountService accountService = AccountService.GetInstance();
                    ApplicationUser user = await accountService.CreateExternalAccount(loginInfo, UserManager);

                    if (user != null)
                        return await ExternalSignIn(loginInfo);
                    else
                        return ControllerHelper.JsonActionResponse(false, "Failed to create account");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TwitterExternalLogin()
        {
            NameValueCollection config = ConfigurationManager.AppSettings;

            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore
                {
                    ConsumerKey = config.Get("twitterID"),
                    ConsumerSecret = config.Get("twitterSecret")
                }
            };

            string callbackUrl = Request.Url.ToString().Replace("ExternalLogin", "TwitterExternalLoginCallback");
            return await auth.BeginAuthorizationAsync(new Uri(callbackUrl));
        }

        [AllowAnonymous]
        public async Task<JsonResult> TwitterExternalLoginCallback()
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore()
            };

            await auth.CompleteAuthorizeAsync(Request.Url);

            var credentials = auth.CredentialStore;
            string oauthToken = credentials.OAuthToken;
            string oauthTokenSecret = credentials.OAuthTokenSecret;
            string screenName = credentials.ScreenName;
            ulong userID = credentials.UserID;
            string data = string.Format("oauth token: {0} Secret: {1} Screen name: {2} User ID: {3}", oauthToken, oauthTokenSecret, screenName, userID);

            var twitterContext = new TwitterContext(auth);
            var verifyResponse = await
                (from acc in twitterContext.Account
                 where (acc.Type == AccountType.VerifyCredentials) && (acc.IncludeEmail == true)
                 select acc)
                 .SingleOrDefaultAsync();

            if (verifyResponse != null && verifyResponse.User != null)
            {
                User twitterUser = verifyResponse.User;
                Debug.WriteLine("EMAIL: " + twitterUser.Email);
            }

            else Debug.WriteLine("verify response or user are NULL");

            return ControllerHelper.JsonActionResponse(true, data);
        }

        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<JsonResult> SendCode(bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return ControllerHelper.JsonActionResponse(false, "[Send code] error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return ControllerHelper.JsonActionResponse(true, "[Send code success]"); //View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return ControllerHelper.JsonActionResponse(true, "[external login] User is already authenticated");
            

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return ControllerHelper.JsonActionResponse(false, "[ExternalLoginConfirm] Login failed");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return ControllerHelper.JsonActionResponse(true, "[External login] User successfully logged in");
                    }

                    else return ControllerHelper.JsonActionResponse(false, "Failed to save login user with email");
                }

                else
                {
                    AddErrors(result);
                    return ControllerHelper.JsonActionResponse(false, "Failed to save login user with email");
                }
            }

            else return ControllerHelper.JsonActionResponse(false, "Invalid input");

//            ViewBag.ReturnUrl = returnUrl;
  //          return View(model);
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task<JsonResult> ExternalSignIn(ExternalLoginInfo loginInfo)
        {
            var signinStatus = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            if(signinStatus == SignInStatus.Success)
                return ControllerHelper.JsonActionResponse(true, "[External sign in] Successfully signed in");

            else
                return ControllerHelper.JsonActionResponse(false, "[External sign in] Failed to sign in"); 
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}