using Microsoft.AspNet.Identity.Owin;
using personal_site.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services
{
    public class AccountService
    {
        private static AccountService _instance;

        private AccountService() { }

        public async Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, ApplicationUserManager userManager)
        {
            string provider = loginInfo.Login.LoginProvider;

            var user = new ApplicationUser()
            {
                UserName = GenerateUsername(loginInfo.Email, provider),
                Email = loginInfo.Email,
                DisplayName = loginInfo.DefaultUserName,
                Provider = loginInfo.Login.LoginProvider
            };

            var createResult = await userManager.CreateAsync(user);

            if (createResult.Succeeded)
            {
                var loginCreateResult = await userManager.AddLoginAsync(user.Id, loginInfo.Login);
                return user;
            }

            else return null;
        }

        private string GenerateUsername(string email, string provider)
        {
            return string.Format("{0}-{1}", provider, email);
        }

        public static AccountService GetInstance()
        {
            _instance = _instance ?? new AccountService();
            return _instance;
        }
    }
}