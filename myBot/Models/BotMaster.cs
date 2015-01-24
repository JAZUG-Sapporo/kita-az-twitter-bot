using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myBot.Models
{
    public class BotMaster
    {
        public int ID { get; set; }

        public string MasterID { get; set; }

        public string BotID { get; set; }

        public virtual Bot Bot { get; set; }
    }
}