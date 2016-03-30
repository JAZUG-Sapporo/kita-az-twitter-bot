using System;
using System.Linq;

namespace myBot.Models.forScripting
{
    /// <summary>
    /// 拡張スクリプト中でアクセス可能なメンバを制限するための、Bot 型に対するラッパーです。
    /// </summary>
    public class BotWrapper
    {
        internal Bot Object { get; private set; }

        public MessageWrapper[] Messages { get; private set; }

        public MessageWrapper MessageToNextTweet
        {
            get { return this.Messages.FirstOrDefault(msg => msg.Object == this.Object.MessageToNextTweet); }
            set { this.Object.MessageToNextTweet = value?.Object; }
        }

        public BotWrapper(Bot bot)
        {
            this.Object = bot;
            this.Messages = bot
                .Messages
                .Select(msg => new MessageWrapper(msg))
                .OrderBy(msg => msg.Order)
                .ToArray();
        }

        public CoreTweet.Status Tweet(string text) => this.Object.Tweet(text);

        public void ArchiveMessage(MessageWrapper message) => this.Object.ArchiveMessage(message.Object);

        public void RestoreMessage(MessageWrapper message) => this.Object.RestoreMessage(message.Object);
    }
}