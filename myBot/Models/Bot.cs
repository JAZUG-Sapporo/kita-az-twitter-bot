using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public Bot()
        {
            this.Enabled = false;
            this.TimeZone = TimeZoneInfo.Utc.Id;
            this.BeginTime = new DateTime(1900, 1, 1);
            this.EndTime = new DateTime(1900, 1, 1);
            this.Duration = 60;
        }
    }
}