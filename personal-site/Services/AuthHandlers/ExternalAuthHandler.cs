﻿using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using personal_site.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var error = createResult.Errors.FirstOrDefault();
          
            if (createResult.Succeeded)
            {
                var loginCreateResult = await userManager.AddLoginAsync(user.Id, loginInfo.Login);
                return user;
            }

            else return null;
        }

        protected async Task SaveAccessTokens(ApplicationUser user, string accessToken, string accessTokenSecret = null)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                UserAccessTokens userToken = new UserAccessTokens()
                {
                    UserId = user.Id,
                    AccessToken = accessToken,
                    AccessTokenSecret = accessTokenSecret
                };

                dbContext.UserAccessTokens.Add(userToken);
                await dbContext.SaveChangesAsync();
            }
        }

        protected ApplicationUser GetDefaultUser(ExternalLoginInfo loginInfo)
        {
            string provider = loginInfo.Login.LoginProvider;

            return new ApplicationUser()
            {
                UserName = GenerateUsername(loginInfo.Email, provider),
                Email = loginInfo.Email,
                DisplayName = GenerateDisplayName(loginInfo.DefaultUserName),
                Provider = provider
            };
        }

        protected async Task<ApplicationUser> SaveUserAndTokens(ApplicationUser user, string accessToken, 
        ExternalLoginInfo loginInfo, ApplicationUserManager userManager, string accessTokenSecret = null)
        {
            ApplicationUser savedUser = await SaveUser(loginInfo, userManager, user);

            if (savedUser != null)
            {
                await SaveAccessTokens(savedUser, accessToken, accessTokenSecret);
                return savedUser;
            }

            else return null;
        }

        protected string GenerateUsername(string email, string provider)
        {
            return string.Format("{0}-{1}", provider, email);
        }

        protected string GenerateDisplayName(string displayName)
        {
            string filteredName = string.Concat(displayName
                .Where(x => Char.IsLetter(x) || Char.IsWhiteSpace(x))
                .Select(x => Char.IsUpper(x) ? (" " + x) : x.ToString()))
                .TrimStart(' ');

            return filteredName;
        }

        public string GetAccessTokenClaim(string tokenName, ExternalLoginInfo loginInfo)
        {
            return loginInfo.ExternalIdentity.Claims
                .Where(c => c.Type.Equals(tokenName))
                .Select(c => c.Value).FirstOrDefault();
        }
    }
}