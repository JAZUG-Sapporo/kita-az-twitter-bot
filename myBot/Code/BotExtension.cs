using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Twitter;
using myBot.Models;
using Newtonsoft.Json;

namespace myBot
{
    public static class BotExtension
    {
        public static Bot GetById(this DbSet<Bot> bots, IPrincipal masterUser, string botID)
        {
            var masterID = masterUser.Identity.Name;
            var bot = bots
                .Where(b => b.BotID == botID)
                .FirstOrDefault(b => b.BotMasters.Any(master => master.MasterID == masterID));
            var twitterAuthOpt = JsonConvert.DeserializeObject<TwitterAuthenticationOptions>(AppSettings.Key.Twitter);
            bot.Init(twitterAuthOpt.ConsumerKey, twitterAuthOpt.ConsumerSecret);
            return bot;
        }

        public static IEnumerable<DateTime> GetTweetTimingsUTC(this Bot bot)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(bot.TimeZone);

            var endTimeLocal = bot.BeginTime < bot.EndTime ? bot.EndTime : bot.EndTime.AddDays(+1);
            var endTimeUtc = TimeZoneInfo.ConvertTimeToUtc(endTimeLocal, timeZoneInfo);
            var time = TimeZoneInfo.ConvertTimeToUtc(bot.BeginTime, timeZoneInfo);
            do
            {
                yield return time;
                time = time.AddMinutes(bot.Duration);
            } while (time <= endTimeUtc);
        }

        public static Message GetMessageToNextTweet(this Bot bot)
        {
            var messages = bot.Messages
                .OrderBy(m => m.Order)
                .ToArray();

            var messageToNextTweet = messages
                .Skip(1)
                .Zip(messages, (m1, m2) => new { m1, m2 })
                .SkipWhile(_ => _.m2.AtLastTweeted < _.m1.AtLastTweeted || _.m1.AtLastTweeted == _.m2.AtLastTweeted)
                .Select(_ => _.m1)
                .DefaultIfEmpty(messages.First())
                .First();

            return messageToNextTweet;
        }
    }
}