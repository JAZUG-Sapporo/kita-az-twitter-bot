using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace myBot.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [Required]
        public string BotID { get; set; }

        [Required]
        public string Text { get; set; }

        public int Order { get; set; }

        public DateTime? AtLastTweeted { get; set; }

        public virtual Bot Bot { get; set; }
    }
}