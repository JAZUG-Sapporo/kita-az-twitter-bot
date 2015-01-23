using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace myBot.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        //
        // POST: /Account/ExternalSignIn
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult ExternalSignIn(string provider, string returnUrl)
        {
#if DEBUG
            if (provider == "demo")
            {
                var signInInfo = new ExternalLoginInfo
                {
                    DefaultUserName = "demo",
                    Email = "demo@example.com",
                    ExternalIdentity = new ClaimsIdentity(new Claim[] { 
                        new Claim(ClaimTypes.NameIdentifier, "abc123")
                    }),
                    Login = new UserLoginInfo("demo", "abc123")
                };
                return ExternalSignInCore(returnUrl, signInInfo);
            }
#endif
            // Request a redirect to the external sign in  provider
            return new ChallengeResult(provider, Url.Action("ExternalSignInCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalSignInCallback
        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> ExternalSignInCallback(string returnUrl)
        {
            var signInInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            return ExternalSignInCore(returnUrl, signInInfo);
        }

        private ActionResult ExternalSignInCore(string returnUrl, ExternalLoginInfo signInInfo)
        {
            if (signInInfo == null) return RedirectToAction("SignIn");

            var login = signInInfo.Login;
            var claimsIdentity = new ClaimsIdentity(signInInfo.ExternalIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie.ToString());
            claimsIdentity.AddClaim(new Claim(CustomClaimTypes.IdentityProvider, login.LoginProvider));
            claimsIdentity.AddClaim(new Claim(CustomClaimTypes.HasedUserId, (login.LoginProvider + "|" + login.ProviderKey + "|" + AppSettings.Salt).ToMD5()));

            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            #region Summary:
            //     Add information to the response environment that will cause the appropriate
            //     authentication middleware to grant a claims-based identity to the recipient
            //     of the response. The exact mechanism of this may vary.  Examples include
            //     setting a cookie, to adding a fragment on the redirect url, or producing
            //     an OAuth2 access code or token response.
            //
            // Parameters:
            //   properties:
            //     Contains additional properties the middleware are expected to persist along
            //     with the claims. These values will be returned as the AuthenticateResult.properties
            //     collection when AuthenticateAsync is called on subsequent requests.
            //
            //   identities:
            //     Determines which claims are granted to the signed in user. The ClaimsIdentity.AuthenticationType
            //     property is compared to the middleware's Options.AuthenticationType value
            //     to determine which claims are granted by which middleware. The recommended
            //     use is to have a single ClaimsIdentity which has the AuthenticationType matching
            //     a specific middleware.
            #endregion
            this.AuthenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            },
            claimsIdentity);

            return RedirectToLocal(returnUrl);
        }

        //
        // POST: /Account/SignOut
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}