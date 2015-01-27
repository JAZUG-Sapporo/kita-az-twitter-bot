using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace myBot.Models
{
    public class MyBotDB : DbContext
    {
        public DbSet<Bot> Bots { get; set; }

        public DbSet<BotMaster> BotMasters { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}