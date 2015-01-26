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

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public virtual List<BotMaster> BotMasters { get; set; }

        public Bot()
        {
            this.Enabled = false;
            this.BeginTime = new DateTime(1900, 1, 1);
            this.EndTime = new DateTime(1900, 1, 1);
            this.Duration = 60;
        }
    }
}