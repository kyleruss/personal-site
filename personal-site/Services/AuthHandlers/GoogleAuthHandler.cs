using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using personal_site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services.AuthHandlers
{
    public class GoogleAuthHandler : ExternalAuthHandler
    {

        public override async Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, 
        ApplicationUserManager userManager,IAuthenticationManager authManager = null)
        {
            string accessToken = GetAccessTokenClaim("urn:google:accesstoken", loginInfo);
            JObject UserDetails = await GetUserDetails(accessToken);

            if (UserDetails == null) return null;
            else
            {
                ApplicationUser user = GetDefaultUser(loginInfo);
                var userImage = UserDetails.GetValue("picture");
                if (userImage != null) user.ProfilePicture = userImage.ToString();

                return await SaveUserAndTokens(user, accessToken, loginInfo, userManager);
            }
        }

        private async Task<JObject> GetUserDetails(string accessToken)
        {
            Uri apiRequestUri = new Uri("https://www.googleapis.com/oauth2/v2/userinfo?access_token=" + accessToken);
            using (var webClient = new WebClient())
            {
                var json = await webClient.DownloadStringTaskAsync(apiRequestUri);
                return JObject.Parse(json);
            }
        }
    }
}