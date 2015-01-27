using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace myBot.Models
{
    public class BotMaster
    {
        [Key]
        public int BotMasterID { get; set; }

        [Required]
        public string MasterID { get; set; }

        [Required]
        public string BotID { get; set; }

        public virtual Bot Bot { get; set; }
    }
}