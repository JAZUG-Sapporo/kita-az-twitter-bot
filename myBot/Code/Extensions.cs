using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using myBot.Models;

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

        public static Bot GetById(this DbSet<Bot> bots, IPrincipal masterUser, string botID)
        {
            var masterID = masterUser.Identity.Name;
            var bot = bots
                .Where(b => b.BotID == botID)
                .FirstOrDefault(b => b.BotMasters.Any(master => master.MasterID == masterID));
            return bot;
        }
    }
}