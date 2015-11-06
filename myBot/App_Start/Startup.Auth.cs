using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Twitter;
using myBot.Controllers;
using Newtonsoft.Json;
using Owin;

namespace myBot
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/"),
                ExpireTimeSpan = TimeSpan.FromDays(365),
                SlidingExpiration = true
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var twitterAuthOpt = JsonConvert.DeserializeObject<TwitterAuthenticationOptions>(AppSettings.Key.Twitter);
            twitterAuthOpt.Provider = new TwitterAuthenticationProvider
            {
                OnApplyRedirect = AccountController.OnTwitterApplyRedirect,
                OnAuthenticated = async context => AccountController.OnTwitterAuthenticated(context)
            };

            // "Owin Twitter login - the remote certificate is invalid according to the validation procedure"
            // http://stackoverflow.com/questions/25011890/owin-twitter-login-the-remote-certificate-is-invalid-according-to-the-validati
            twitterAuthOpt.BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(
                new[]{
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                    "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", // VeriSign Class 3 Primary CA - G5
                    "5168FF90AF0207753CCCD9656462A212B859723B", // DigiCert SHA2 High Assurance Server C‎A 
                    "B13EC36903F8BF4701D498261A0802EF63642BC3" // DigiCert High Assurance EV Root CA
                });
            app.UseTwitterAuthentication(twitterAuthOpt);
        }
    }
}