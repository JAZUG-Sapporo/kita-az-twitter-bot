using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace myBot
{
    public static class Extensions
    {
        public static string AppUrl(this Uri requestUrl)
        {
            return requestUrl.GetLeftPart(UriPartial.Scheme | UriPartial.Authority);
        }

        public static string ValueOf(this IEnumerable<Claim> claims, string type)
        {
            var claim = claims.FirstOrDefault(c => c.Type == type);
            return claim != null ? claim.Value : "";
        }

        public static bool In<T>(this T value, params T[] options) where T : IEquatable<T>
        {
            return options.Any(opt => opt.Equals(value));
        }

        public static IEnumerable<string> Except(this IEnumerable<string> first, params string[] seconds)
        {
            return Enumerable.Except(first, seconds);
        }
    }
}