using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myBot
{
    public static class CustomClaimTypes
    {
        public const string IdentityProvider = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

        public static class Twitter
        {
            public const string AccessToken = "urn:twitter:access_token";

            public const string AccessTokenSecret = "urn:twitter:access_token_secret";
        }
    }
}