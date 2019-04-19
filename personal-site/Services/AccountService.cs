using LinqToTwitter;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using personal_site.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services
{
    public class AccountService
    {
        private static AccountService _instance;

        private AccountService() { }

        public async Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, ApplicationUserManager userManager, ApplicationUser user = null)
        {
            string provider = loginInfo.Login.LoginProvider;


            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = GenerateUsername(loginInfo.Email, provider),
                    Email = loginInfo.Email,
                    DisplayName = loginInfo.DefaultUserName,
                    Provider = provider
                };
            }

            var createResult = await userManager.CreateAsync(user);

            if (createResult.Succeeded)
            {
                var loginCreateResult = await userManager.AddLoginAsync(user.Id, loginInfo.Login);
                return user;
            }

            else return null;
        }

        public async Task<ApplicationUser> CreateTwitterAccount(ExternalLoginInfo loginInfo, ApplicationUserManager userManager, IAuthenticationManager authManager)
        {
            SessionStateCredentialStore credentials = await GetCredentialStore(authManager);
            if (credentials == null) return null;

            else
            {
                var auth = new AspNetAuthorizer
                {
                    CredentialStore = credentials
                };

                var twitterContext = new TwitterContext(auth);

                var accResponse = await
                (from acc in twitterContext.Account
                    where (acc.Type == AccountType.VerifyCredentials) && (acc.IncludeEmail == true)
                    select acc)
                    .SingleOrDefaultAsync();

                if (accResponse != null && accResponse.User != null)
                {
                    User responseUser = accResponse.User;
                    string userEmail = responseUser.Email;
                    string provider = loginInfo.Login.LoginProvider;

                    ApplicationUser twitterUser = new ApplicationUser()
                    {
                        UserName = GenerateUsername(userEmail, provider),
                        Email = userEmail,
                        DisplayName = responseUser.Name,
                        Provider = provider
                    };

                    ApplicationUser savedUser = await CreateExternalAccount(loginInfo, userManager, twitterUser);
                    if (savedUser != null)
                        await SaveTwitterAccessTokens(twitterUser, credentials);

                    return savedUser;
                }

                else return null;
            }
        }

        private async Task SaveTwitterAccessTokens(ApplicationUser twitterUser, SessionStateCredentialStore credentials)
        {
            using(ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                UserAccessTokens userToken = new UserAccessTokens()
                {
                        UserId = twitterUser.Id,
                        AccessToken = credentials.OAuthToken,
                        AccessTokenSecret = credentials.OAuthTokenSecret
                };

                dbContext.UserAccessTokens.Add(userToken);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task<SessionStateCredentialStore> GetCredentialStore(IAuthenticationManager authManager)
        {
            var claims = await authManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            if (claims == null) return null;

            else
            {
                Claim accessToken = claims.FindFirst(LinqToTwitterAuthenticationProvider.AccessToken);
                Claim accessTokenSecret = claims.FindFirst(LinqToTwitterAuthenticationProvider.AccessTokenSecret);
                NameValueCollection config = ConfigurationManager.AppSettings;

                return new SessionStateCredentialStore
                {
                    ConsumerKey = config.Get("twitterID"),
                    ConsumerSecret = config.Get("twitterSecret"),
                    OAuthToken = accessToken.Value,
                    OAuthTokenSecret = accessTokenSecret.Value
                }; 
            }
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