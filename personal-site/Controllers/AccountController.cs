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
        public ActionResult ExternalLogin(string provider)
        {
            ControllerContext.HttpContext.Session.RemoveAll();

            // Request a redirect to the external login provider
             return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account"));
        }


        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<JsonResult> ExternalLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

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
                case SignInStatus.Failure:
                default:
                    AccountService accountService = AccountService.GetInstance();
                    ApplicationUser user;
                    
                    if(loginInfo.Login.LoginProvider == "Twitter")
                        user = await accountService.CreateTwitterAccount(loginInfo, UserManager, AuthenticationManager);
                    else
                        user = await accountService.CreateExternalAccount(loginInfo, UserManager);

                    if (user != null)
                        return await ExternalSignIn(loginInfo);
                    else
                        return ControllerHelper.JsonActionResponse(false, "Failed to create account");
            }
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