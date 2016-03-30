using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace myBot.Models
{
    public class Bot
    {
        [Key, Required]
        public string BotID { get; set; }

        public string AccessToken { get; set; }

        public string AccessTokenSecret { get; set; }

        public bool Enabled { get; set; }

        [Required, UIHint("TimeZone")]
        public string TimeZone { get; set; }

        [UIHint("Time")]
        public DateTime BeginTime { get; set; }

        [UIHint("Time")]
        public DateTime EndTime { get; set; }

        [UIHint("Duration")]
        public int Duration { get; set; }

        public virtual List<BotMaster> BotMasters { get; set; }

        public virtual List<Message> Messages { get; set; }

        public virtual List<MessageJournal> MessageJournals { get; set; }

        public virtual List<ExtensionScript> ExtensionScripts { get; set; }

        [NotMapped]
        public Message MessageToNextTweet { get; set; }

        public Func<string, CoreTweet.Status> HookTweet { get; set; }

        public Bot()
        {
            this.Enabled = false;
            this.TimeZone = TimeZoneInfo.Utc.Id;
            this.BeginTime = new DateTime(1900, 1, 1);
            this.EndTime = new DateTime(1900, 1, 1);
            this.Duration = 60;
        }

        [NotMapped]
        private string ConsumerKey { get; set; }

        [NotMapped]
        private string ConsumerSecret { get; set; }

        [NotMapped]
        private CoreTweet.Tokens _Token;

        [NotMapped]
        private CoreTweet.Tokens Token
        {
            get
            {
                lock (this)
                {
                    if (this._Token == null)
                    {
                        this._Token = CoreTweet.Tokens.Create(
                            this.ConsumerKey,
                            this.ConsumerSecret,
                            this.AccessToken,
                            this.AccessTokenSecret);
                    }
                }
                return this._Token;
            }
        }

        internal void Init(string consumerKey, string consumerSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
        }

        public CoreTweet.Status Tweet(string text)
        {
            return this.HookTweet != null ?
                this.HookTweet(text) :
                this.TweetAsync(text).Result;
        }

        public async Task<CoreTweet.Status> TweetAsync(string text)
        {
            try
            {
                var result = default(CoreTweet.Status);
                if (this.HookTweet != null)
                {
                    result = HookTweet(text);
                }
                else
                {
                    result = await this.Token.Statuses.UpdateAsync(status => text);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Unhandled exception when tweeting '{text}'", innerException: e);
            }
        }

        /// <summary>
        /// 引数に指定されたメッセージをアーカイブします。(拡張スクリプト中からも呼び出されることを想定しています)
        /// </summary>
        public void ArchiveMessage(Message messageToArchive)
        {
            messageToArchive.IsArchived = true;

            if (this.MessageToNextTweet.MessageID == messageToArchive.MessageID)
            {
                this.MessageToNextTweet = this.GetMessageToNextTweet();
            }
        }

        /// <summary>
        /// 引数に指定されたメッセージをアーカイブから戻します。(拡張スクリプト中からも呼び出されることを想定しています)
        /// </summary>
        public void RestoreMessage(Message messageToRestore)
        {
            messageToRestore.IsArchived = false;
        }
    }
}