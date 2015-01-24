using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myBot.Models
{
    public class Bot
    {
        public string BotID { get; set; }

        public string AccessToken { get; set; }

        public string AccessSecret { get; set; }

        public bool Enabled { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public virtual List<BotMaster> BotMasters { get; set; }
    }
}