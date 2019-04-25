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
    public class MicrosoftAuthHandler : ExternalAuthHandler
    {

        public override Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, 
        ApplicationUserManager userManager,IAuthenticationManager authManager = null)
        {
            throw new NotImplementedException();
        }

        public string GetAccessToken(ExternalLoginInfo loginInfo)
        {
            return GetAccessTokenClaim("access_token", loginInfo);
        }
    }
}