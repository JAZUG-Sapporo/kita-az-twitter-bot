using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNet.Identity;

namespace myBot
{
    public static class Extensions
    {
        public static string ToMD5(this string text)
        {
            var md5Bytes = new MD5Cng().ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(md5Bytes).Replace("-", "").ToLower();
        }

        public static string GetHashedUserId(this IPrincipal principal)
        {
            if (principal == null) return null;
            var claimsIdentty = principal.Identity as ClaimsIdentity;
            if (claimsIdentty == null) return null;
            return claimsIdentty.FindFirstValue(CustomClaimTypes.HasedUserId);
        }

        public static string AppUrl(this Uri requestUrl)
        {
            return requestUrl.GetLeftPart(UriPartial.Scheme | UriPartial.Authority);
        }

        public static string ValueOf(this IEnumerable<Claim> claims, string type)
        {
            var claim = claims.FirstOrDefault(c => c.Type == type);
            return claim != null ? claim.Value : "";
        }
    }
}