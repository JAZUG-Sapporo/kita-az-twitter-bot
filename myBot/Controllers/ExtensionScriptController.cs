using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using myBot.Models;

namespace myBot.Controllers
{
    public class ExtensionScriptController : Controller
    {
        public MyBotDB DB { get; set; }

        public ExtensionScriptController()
        {
            this.DB = new MyBotDB();
        }

        // GET: foo/ExtensionScript/Create
        public ActionResult Create(string id)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            return View(new ExtensionScript { BotID = bot.BotID, Bot = bot });
        }

        // POST: foo/ExtensionScript/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string id, ExtensionScript model)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            model.BotID = id;
            model.Bot = bot;
            if (ModelState.IsValid)
            {
                bot.ExtensionScripts.Add(model);
                await this.DB.SaveChangesAsync();
                return RedirectToAction("Details", "Bot", new { id });
            }
            else
            {
                return View(model);
            }
        }

        // GET: foo/ExtensionScript/Edit/5
        public ActionResult Edit(string id, int scriptID)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var script = bot.ExtensionScripts.FirstOrDefault(s => s.ScriptID == scriptID);
            if (script == null) return HttpNotFound();
            return View(script);
        }

        // POST: foo/ExtensionScript/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, int scriptID, ExtensionScript model)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var script = bot.ExtensionScripts.FirstOrDefault(s => s.ScriptID == scriptID);
            if (script == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                UpdateModel(script, "Title,Language,TargetEvent,ScriptBody,Enabled".Split(','));
                await this.DB.SaveChangesAsync();
                return RedirectToAction("Details", "Bot", new { id });
            }
            else
            {
                return View(script);
            }
        }

        // POST: foo/ExtensionScript/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string id, int scriptID)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var script = bot.ExtensionScripts.FirstOrDefault(s => s.ScriptID == scriptID);
            if (script == null) return HttpNotFound();

            this.DB.ExtensionScripts.Remove(script);
            await this.DB.SaveChangesAsync();

            return new EmptyResult();
        }
    }
}
