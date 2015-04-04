using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myBot.Code;
using myBot.Models;
using Toolbelt;

namespace myBot.Test
{
    [TestClass]
    public class BotExtensionTest
    {
        [TestMethod]
        public void GetTweetTimings_JustEndTime_Test()
        {
            var bot = new Bot
            {
                BeginTime = new DateTime(1900, 1, 1, 21, 0, 0),
                EndTime = new DateTime(1900, 1, 1, 22, 30, 0),
                Duration = 45
            };
            bot.GetTweetTimingsUTC().Is(
                new TimeSpan(21, 0, 0),
                new TimeSpan(21, 45, 0),
                new TimeSpan(22, 30, 0)
            );
        }

        [TestMethod]
        public void GetTweetTimings_OverEndTime_Test()
        {
            var bot = new Bot
            {
                BeginTime = new DateTime(1900, 1, 1, 21, 0, 0),
                EndTime = new DateTime(1900, 1, 1, 23, 30, 0),
                Duration = 45
            };
            bot.GetTweetTimingsUTC().Is(
                new TimeSpan(21, 0, 0),
                new TimeSpan(21, 45, 0),
                new TimeSpan(22, 30, 0),
                new TimeSpan(23, 15, 0)
            );
        }

        [TestMethod]
        public void GetTweetTimings_OverDay_Test()
        {
            var bot = new Bot
            {
                BeginTime = new DateTime(1900, 1, 1, 23, 0, 0),
                EndTime = new DateTime(1900, 1, 1, 9, 0, 0),
                Duration = 120
            };
            bot.GetTweetTimingsUTC().Is(
                new TimeSpan(23, 0, 0),
                new TimeSpan(1, 0, 0),
                new TimeSpan(3, 0, 0),
                new TimeSpan(5, 0, 0),
                new TimeSpan(7, 0, 0),
                new TimeSpan(9, 0, 0)
            );
        }

        [TestMethod]
        public void GetTweetTimings_OverDay_TimeZone_Test()
        {
            var bot = new Bot
            {
                TimeZone = "Tokyo Standard Time",
                BeginTime = new DateTime(1900, 1, 1, 23, 0, 0),
                EndTime = new DateTime(1900, 1, 1, 9, 0, 0),
                Duration = 120
            };
            bot.GetTweetTimingsUTC().Is(
                new TimeSpan(14, 0, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(18, 0, 0),
                new TimeSpan(20, 0, 0),
                new TimeSpan(22, 0, 0),
                new TimeSpan(0, 0, 0)
            );
        }

        [TestMethod]
        public void GetTweetTimings_AllDay_Test()
        {
            var bot = new Bot
            {
                BeginTime = new DateTime(1900, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1900, 1, 1, 0, 0, 0),
                Duration = 60
            };
            bot.GetTweetTimingsUTC().Is(
                new TimeSpan(0, 0, 0),
                new TimeSpan(1, 0, 0),
                new TimeSpan(2, 0, 0),
                new TimeSpan(3, 0, 0),
                new TimeSpan(4, 0, 0),
                new TimeSpan(5, 0, 0),
                new TimeSpan(6, 0, 0),
                new TimeSpan(7, 0, 0),
                new TimeSpan(8, 0, 0),
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(13, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 0, 0),
                new TimeSpan(18, 0, 0),
                new TimeSpan(19, 0, 0),
                new TimeSpan(20, 0, 0),
                new TimeSpan(21, 0, 0),
                new TimeSpan(22, 0, 0),
                new TimeSpan(23, 0, 0),
                new TimeSpan(0, 0, 0)
            );
        }

        [TestMethod]
        public void GetMessageToNextTweet_AllNull_Test()
        {
            var bot = new Bot { Messages = new List<Message>() };
            bot.Messages.AddRange(new[] { 
                new Message { MessageID = 1, AtLastTweeted = null, Text = "Message-A" },
                new Message { MessageID = 2, AtLastTweeted = null, Text = "Message-B" },
                new Message { MessageID = 3, AtLastTweeted = null, Text = "Message-C" },
            });

            bot.GetMessageToNextTweet().MessageID.Is(1);
        }

        [TestMethod]
        public void GetMessageToNextTweet_ContainsNull_Test()
        {
            var bot = new Bot { Messages = new List<Message>() };
            bot.Messages.AddRange(new[] { 
                new Message { MessageID = 1, AtLastTweeted = new DateTime(2015, 1, 27, 23, 0, 0), Text = "Message-A" },
                new Message { MessageID = 2, AtLastTweeted = new DateTime(2015, 1, 28, 1, 0, 0), Text = "Message-B" },
                new Message { MessageID = 3, AtLastTweeted = null, Text = "Message-C" },
                new Message { MessageID = 4, AtLastTweeted = null, Text = "Message-D" },
            });

            bot.GetMessageToNextTweet().MessageID.Is(3);
        }

        [TestMethod]
        public void GetMessageToNextTweet_JustEndLast_Test()
        {
            var bot = new Bot { Messages = new List<Message>() };
            bot.Messages.AddRange(new[] { 
                new Message { MessageID = 1, AtLastTweeted = new DateTime(2015, 1, 27, 23, 0, 0), Text = "Message-A" },
                new Message { MessageID = 2, AtLastTweeted = new DateTime(2015, 1, 28, 1, 0, 0), Text = "Message-B" },
                new Message { MessageID = 3, AtLastTweeted = new DateTime(2015, 1, 28, 3, 0, 0), Text = "Message-C" },
                new Message { MessageID = 4, AtLastTweeted = new DateTime(2015, 1, 28, 5, 0, 0), Text = "Message-D" },
            });

            bot.GetMessageToNextTweet().MessageID.Is(1);
        }

        [TestMethod]
        public void GetMessageToNextTweet_RollOvered_Test()
        {
            var bot = new Bot { Messages = new List<Message>() };
            bot.Messages.AddRange(new[] { 
                new Message { MessageID = 1, AtLastTweeted = new DateTime(2015, 1, 28, 7, 0, 0), Text = "Message-A" },
                new Message { MessageID = 2, AtLastTweeted = new DateTime(2015, 1, 28, 1, 0, 0), Text = "Message-B" },
                new Message { MessageID = 3, AtLastTweeted = new DateTime(2015, 1, 28, 3, 0, 0), Text = "Message-C" },
                new Message { MessageID = 4, AtLastTweeted = new DateTime(2015, 1, 28, 5, 0, 0), Text = "Message-D" },
            });

            bot.GetMessageToNextTweet().MessageID.Is(2);
        }

        [TestMethod]
        public void GetMessageToNextTweet_ChangedOrder_Test()
        {
            var bot = new Bot { Messages = new List<Message>() };
            bot.Messages.AddRange(new[] { 
                new Message { MessageID = 1, AtLastTweeted = new DateTime(2015, 1, 27, 23, 0, 0), Text = "Message-A" },
                new Message { MessageID = 2, AtLastTweeted = new DateTime(2015, 1, 28, 3, 0, 0), Text = "Message-C" },
                new Message { MessageID = 3, AtLastTweeted = new DateTime(2015, 1, 28, 1, 0, 0), Text = "Message-B" },
                new Message { MessageID = 4, AtLastTweeted = new DateTime(2015, 1, 28, 5, 0, 0), Text = "Message-D" },
            });

            bot.GetMessageToNextTweet().MessageID.Is(3);
        }

        [TestMethod]
        public void GenerateAvoidingTextPatterns_Test()
        {
            BotExtension.GenerateAvoidingTextPatterns("").Is("");

            BotExtension.GenerateAvoidingTextPatterns("A").Is("A");

            BotExtension.GenerateAvoidingTextPatterns("ABC").Is(
                "ABC",
                "A\u200bBC",
                "AB\u200bC",
                "A\u200bB\u200bC");
        }
    }
}
