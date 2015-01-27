using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using myBot.Models;

namespace myBot.Controllers
{
    [Authorize]
    public class BotMasterController : Controller
    {
        public MyBotDB DB { get; set; }

        public BotMasterController()
        {
            this.DB = new MyBotDB();
        }

        [HttpGet]
        public ActionResult Create(string id)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            return View(new BotMaster { BotID = bot.BotID, Bot = bot });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string id, BotMaster botmaster)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                bot.BotMasters.Add(new BotMaster
                {
                    MasterID = botmaster.MasterID
                });
                await this.DB.SaveChangesAsync();
                return RedirectToAction("Details", "Bot", new { id });
            }
            else
            {
                return View(new BotMaster { BotID = bot.BotID, Bot = bot });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id, string masterID)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var masterToDelete = bot.BotMasters.FirstOrDefault(m => m.MasterID == masterID);
            if (masterToDelete == null) return HttpNotFound();

            this.DB.BotMasters.Remove(masterToDelete);
            await this.DB.SaveChangesAsync();

            return new EmptyResult();
        }
    }
}