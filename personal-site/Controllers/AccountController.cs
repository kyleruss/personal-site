using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using personal_site.Models;
using personal_site.Services;
using personal_site.Services.AuthHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Controllers
{
    [Authorize]
    public class AccountController : AbstractAuthController
    {
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
        public async Task<ActionResult> ExternalLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
                return RedirectToAction("SocialAuthCallback", "Blog", new { message = "You need to login" });
            

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("SocialAuthCallback", "Blog", new { message = "Successfully signed in" });
                case SignInStatus.LockedOut:
                    return RedirectToAction("SocialAuthCallback", "Blog", new { message = "Locked out" });
                case SignInStatus.Failure:
                default:
                    AccountService accountService = AccountService.GetInstance();
                    ExternalAuthHandler authHandler = accountService.GetExternalAuthHandler(loginInfo);

                    ApplicationUser savedUser = await authHandler.CreateExternalAccount(loginInfo, UserManager, AuthenticationManager);

                    if (savedUser != null)
                        return await ExternalSignIn(loginInfo);
                    else
                        return RedirectToAction("SocialAuthCallback", "Blog", new { message = "Failed to create account" });
            }
        }
  
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private async Task<ActionResult> ExternalSignIn(ExternalLoginInfo loginInfo)
        {
            var signinStatus = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            if(signinStatus == SignInStatus.Success)
                return RedirectToAction("SocialAuthCallback", "Blog", new { message = "[External signin] Successfully signed in" });

            else
                return RedirectToAction("SocialAuthCallback", "Blog", new { message = "[External signin] Failed to sign in" });
        }
   
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri): this(provider, redirectUri, null) { }

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
                    properties.Dictionary[XsrfKey] = UserId;

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}