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