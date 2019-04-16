using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using personal_site.Models;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Twitter;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Facebook;
using System.Security.Claims;

namespace personal_site
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            NameValueCollection config = ConfigurationManager.AppSettings;

            //===================================================
            //  SOCIAL MEDIA AUTHENTICATION
            //===================================================

            var microsoftOptions = new MicrosoftAccountAuthenticationOptions
            {
                ClientId = config.Get("microsoftID"),
                ClientSecret = config.Get("microsoftSecret")
            };

            microsoftOptions.Scope.Add("wl.basic");
            microsoftOptions.Scope.Add("wl.emails");

            app.UseMicrosoftAccountAuthentication(microsoftOptions);

            app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = config.Get("twitterID"),
                ConsumerSecret = config.Get("twitterSecret"),
              
                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[]
                {
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                    "5168FF90AF0207753CCCD9656462A212B859723B", //DigiCert SHA2 High Assurance Server C‎A 
                    "B13EC36903F8BF4701D498261A0802EF63642BC3" //DigiCert High Assurance EV Root CA
                })
            });

            var facebookOptions = new FacebookAuthenticationOptions
            {
                AppId = config.Get("facebookID"),
                AppSecret = config.Get("facebookSecret"),

            };

            facebookOptions.Scope.Add("email");
            facebookOptions.Fields.Add("email");

            facebookOptions.Scope.Add("public_profile");
            facebookOptions.Fields.Add("name");
            app.UseFacebookAuthentication(facebookOptions);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = config.Get("googleID"),
                ClientSecret = config.Get("googleSecret")
            });
        }
    }
}