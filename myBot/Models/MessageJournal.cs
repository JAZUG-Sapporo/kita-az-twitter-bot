using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myBot.Models
{
    public class MessageJournal
    {
        [Key]
        public int MessageJournalID { get; set; }

        [Required]
        public string BotID { get; set; }

        [Required, AllowHtml]
        public string Text { get; set; }

        public DateTime TweetAt { get; set; }
    }
}