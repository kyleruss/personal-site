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
    public class GoogleAuthHandler : ExternalAuthHandler
    {

        public override Task<ApplicationUser> CreateExternalAccount(ExternalLoginInfo loginInfo, 
        ApplicationUserManager userManager,IAuthenticationManager authManager = null)
        {
            throw new NotImplementedException();
        }

        private string GetAccessToken(ExternalLoginInfo loginInfo)
        {
            return loginInfo.ExternalIdentity.Claims
                .Where(c => c.Type.Equals("urn:google:accesstoken"))
                .Select(c => c.Value).FirstOrDefault();
        }
    }
}