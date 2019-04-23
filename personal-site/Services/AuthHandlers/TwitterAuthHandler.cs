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

namespace personal_site.Services.AuthHandlers
{
    public class TwitterAuthHandler : ExternalAuthHandler
    {
        public override async Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, 
            ApplicationUserManager userManager, IAuthenticationManager authManager = null)
        {
            SessionStateCredentialStore credentials = await GetTwitterCredentialStore(authManager);
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
                        Provider = provider,
                        ProfilePicture = responseUser.ProfileImageUrl
                    };

                    ApplicationUser savedUser = await SaveUser(loginInfo, userManager, twitterUser);
                    if (savedUser != null)
                        await SaveAccessTokens(twitterUser, credentials);

                    return savedUser;
                }

                else return null;
            }
        }

        private async Task SaveAccessTokens(ApplicationUser user, SessionStateCredentialStore credentials)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                UserAccessTokens userToken = new UserAccessTokens()
                {
                    UserId = user.Id,
                    AccessToken = credentials.OAuthToken,
                    AccessTokenSecret = credentials.OAuthTokenSecret
                };

                dbContext.UserAccessTokens.Add(userToken);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task<SessionStateCredentialStore> GetTwitterCredentialStore(IAuthenticationManager authManager)
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
    }
}