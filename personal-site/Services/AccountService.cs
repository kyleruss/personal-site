using LinqToTwitter;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using personal_site.Models;
using personal_site.Services.AuthHandlers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services
{
    public class AccountService
    {
        public const string SESSION_CODE_NAME = "sys_authcode";
        public const string SESSION_USER_NAME = "sys_authuser";

        private static AccountService _instance;

        private AccountService() { }

        public async Task<bool> SendAuthCode()
        {
            MailService mailService = MailService.GetInstance();
            string authCode = Guid.NewGuid().ToString();

            HttpContext.Current.Session[SESSION_CODE_NAME] = authCode;

            MailMessage message = mailService.PrepareAuthCodeMessage(authCode);
            return await mailService.SendMessage(message);
        }

        public async Task<bool> VerifyAuthCode(string userAuthCode, ApplicationSignInManager signInManager)
        {
            var session = HttpContext.Current.Session;
            var systemAuthCode = session[SESSION_CODE_NAME];
            var systemUser = session[SESSION_USER_NAME];

            if (systemAuthCode == null || systemUser == null || userAuthCode == null)
                return false;
            else
            { 
                bool authStatus = userAuthCode.Equals(systemAuthCode);

                if (authStatus)
                {
                    ApplicationUser user = (ApplicationUser) systemUser;
                    await signInManager.SignInAsync(user, false, false);

                    session.Remove(SESSION_CODE_NAME);
                    session.Remove(SESSION_USER_NAME);
                }

                return authStatus;
            }
        }

        public async Task<bool> VerifySystemUser(string username, string password, ApplicationUserManager userManager)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);

            if (user == null) return false;

            bool validStatus = await userManager.CheckPasswordAsync(user, password);
            bool roleStatus = await userManager.IsInRoleAsync(user.Id, "Admin");

            if (validStatus && roleStatus)
            {
                HttpContext.Current.Session[SESSION_USER_NAME] = user;
                return true;
            }

            else return false;
        }

        public bool AuthCodePending()
        {
            var session = HttpContext.Current.Session;
            return session[SESSION_CODE_NAME] != null && session[SESSION_USER_NAME] != null;
        }

        public ExternalAuthHandler GetExternalAuthHandler(ExternalLoginInfo loginInfo)
        {
            string provider = loginInfo.Login.LoginProvider;

            switch(provider)
            {
                case "Google":
                    return new GoogleAuthHandler();

                case "Twitter":
                    return new TwitterAuthHandler();

                case "Facebook":
                    return new FacebookAuthHandler();

                case "Microsoft":
                    return new MicrosoftAuthHandler();

                default: return null;
            }
        }

        public static AccountService GetInstance()
        {
            _instance = _instance ?? new AccountService();
            return _instance;
        }
    }
}