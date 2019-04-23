using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using personal_site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services.AuthHandlers
{
    public abstract class ExternalAuthHandler
    {
        public abstract Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, 
            ApplicationUserManager userManager, IAuthenticationManager authManager = null);


        protected async Task<ApplicationUser> SaveUser(ExternalLoginInfo loginInfo, ApplicationUserManager userManager, ApplicationUser user = null)
        {
            string provider = loginInfo.Login.LoginProvider;

            if (user == null)
                user = GetDefaultUser(loginInfo);

            var createResult = await userManager.CreateAsync(user);

            if (createResult.Succeeded)
            {
                var loginCreateResult = await userManager.AddLoginAsync(user.Id, loginInfo.Login);
                return user;
            }

            else return null;
        }

        protected ApplicationUser GetDefaultUser(ExternalLoginInfo loginInfo)
        {
            string provider = loginInfo.Login.LoginProvider;

            return new ApplicationUser()
            {
                UserName = GenerateUsername(loginInfo.Email, provider),
                Email = loginInfo.Email,
                DisplayName = loginInfo.DefaultUserName,
                Provider = provider
            };
        }

        protected string GenerateUsername(string email, string provider)
        {
            return string.Format("{0}-{1}", provider, email);
        }

    }
}