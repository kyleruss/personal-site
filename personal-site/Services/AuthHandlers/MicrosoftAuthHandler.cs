using Microsoft.AspNet.Identity.Owin;
using Microsoft.Graph;
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
        ApplicationUserManager userManager, IAuthenticationManager authManager = null)
        {
            ApplicationUser user = GetDefaultUser(loginInfo);
            return await SaveUser(loginInfo, userManager, user);
        }
    }
}