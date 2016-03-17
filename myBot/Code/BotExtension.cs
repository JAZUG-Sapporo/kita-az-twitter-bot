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

        public static IEnumerable<TimeSpan> GetTweetTimingsUTC(this Bot bot)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(bot.TimeZone);

            var endTimeLocal = bot.BeginTime < bot.EndTime ? bot.EndTime : bot.EndTime.AddDays(+1);
            var endTimeUtc = TimeZoneInfo.ConvertTimeToUtc(endTimeLocal, timeZoneInfo);
            var time = TimeZoneInfo.ConvertTimeToUtc(bot.BeginTime, timeZoneInfo);
            do
            {
                yield return time.TimeOfDay;
                time = time.AddMinutes(bot.Duration);
            } while (time <= endTimeUtc);
        }

        public static IEnumerable<Message> GetAvailableMessages(this Bot bot)
        {
            return bot.Messages.OfAvailable();
        }

        public static IEnumerable<Message> OfAvailable(this IEnumerable<Message> messages)
        {
            return messages.Where(m => !m.IsArchived);
        }

        public static Message GetMessageToNextTweet(this Bot bot)
        {
            var messages = bot.GetAvailableMessages()
                .OrderBy(m => m.Order)
                .ToArray();

            var messageToNextTweet = messages
                .Skip(1)
                .Zip(messages, (m1, m2) => new { m1, m2 })
                .SkipWhile(_ => _.m2.AtLastTweeted < _.m1.AtLastTweeted || _.m1.AtLastTweeted == _.m2.AtLastTweeted)
                .Select(_ => _.m1)
                .DefaultIfEmpty(messages.FirstOrDefault())
                .FirstOrDefault();

            return messageToNextTweet;
        }

        public static string AvoidDupulicateText(this Bot bot, string text)
        {
            var jornals = bot.MessageJournals
                .OrderByDescending(msg => msg.TweetAt)
                .Take(10)
                .ToArray();

            var orgText = text;
            text = GenerateAvoidingTextPatterns(text)
                .FirstOrDefault(t => jornals.All(j => t != j.Text));
            if (text == null) return orgText;

            bot.MessageJournals.Add(new MessageJournal
            {
                Text = text,
                TweetAt = DateTime.UtcNow
            });

            return text;
        }

        public static IEnumerable<string> GenerateAvoidingTextPatterns(string text)
        {
            var list = text.ToCharArray();
            return GenerateAvoidingTextPatterns(list.FirstOrDefault(), list.Skip(1));
        }

        private static IEnumerable<string> GenerateAvoidingTextPatterns(char head, IEnumerable<char> tails)
        {
            if (head == default(char))
            {
                yield return "";
                yield break;
            }
            if (tails.Any() == false)
            {
                yield return head.ToString();
                yield break;
            }
            foreach (var item in GenerateAvoidingTextPatterns(tails.First(), tails.Skip(1)))
            {
                yield return head + item;
                yield return head + "\u200b" + item;
            }
        }
    }
}