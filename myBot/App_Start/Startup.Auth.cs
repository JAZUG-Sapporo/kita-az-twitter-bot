using System;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
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
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var twitterAuthOpt = JsonConvert.DeserializeObject<TwitterAuthenticationOptions>(AppSettings.Key.Twitter);
            twitterAuthOpt.Provider = new TwitterAuthenticationProvider
            {
                OnApplyRedirect = AccountController.OnTwitterApplyRedirect,
                OnAuthenticated = async context => AccountController.OnTwitterAuthenticated(context)
            };
            app.UseTwitterAuthentication(twitterAuthOpt);
        }
    }
}