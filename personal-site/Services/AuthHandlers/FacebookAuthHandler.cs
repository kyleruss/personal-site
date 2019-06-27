using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using personal_site.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services.AuthHandlers
{
    public class FacebookAuthHandler : ExternalAuthHandler
    {
        public override async Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, 
        ApplicationUserManager userManager, IAuthenticationManager authManager = null)
        {
            string accessToken = GetAccessTokenClaim("FacebookAccessToken", loginInfo);
            string userId = await GetUserId(loginInfo, accessToken);
            string profileImage = string.Format("https://graph.facebook.com/{0}/picture", userId);

            ApplicationUser fbUser = GetDefaultUser(loginInfo);
            fbUser.UserName = GenerateUsername(userId, "Facebook");
            fbUser.ProfilePicture = profileImage;

            return await SaveUserAndTokens(fbUser, accessToken, loginInfo, userManager);
        }

        private async Task<string> GetUserId(ExternalLoginInfo loginInfo, string accessToken)
        {
            FacebookClient fbClient = new FacebookClient(accessToken);
            object fbUserInfo = await fbClient.GetTaskAsync("/me");

            if (fbUserInfo == null) return null;
            JObject fbUserObj = JObject.Parse(fbUserInfo.ToString());
            return fbUserObj.GetValue("id").ToString();
        }
    }
}