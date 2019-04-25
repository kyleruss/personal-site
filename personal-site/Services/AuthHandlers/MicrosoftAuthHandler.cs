using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using personal_site.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services.AuthHandlers
{
    public class MicrosoftAuthHandler : ExternalAuthHandler
    {

        public override async Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, 
        ApplicationUserManager userManager,IAuthenticationManager authManager = null)
        {
            string accessToken = GetAccessTokenClaim("access_token", loginInfo);
            string profileImage = await GetUserProfileImage(loginInfo, accessToken);

            ApplicationUser user = GetDefaultUser(loginInfo);
            user.ProfilePicture = profileImage;

            return await SaveUserAndTokens(user, accessToken, loginInfo, userManager);
        }

        //display with <img src="data:image/jpeg;base64, *imgbase64encodedstring*" />
        public async Task<string> GetUserProfileImage(ExternalLoginInfo loginInfo, string accessToken)
        {
            Uri requestUri = new Uri("https://graph.microsoft.com/v1.0/me/photo/$value");

            using (var webClient = new HttpClient())
            {
                webClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage data = await webClient.GetAsync(requestUri);
                byte[] imgData = await data.Content.ReadAsByteArrayAsync();
                string imgString = Convert.ToBase64String(imgData);
                    

                return imgString;
            }
            
        }

    }
}