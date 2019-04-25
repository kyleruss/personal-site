using Microsoft.AspNet.Identity;
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
    public class FacebookAuthHandler : ExternalAuthHandler
    {
        public override async Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, 
        ApplicationUserManager userManager, IAuthenticationManager authManager = null)
        {
           // string accessToken = await GetAccessToken(authManager);
            return null;
        }
    }
}