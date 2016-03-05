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
    public class MessageController : Controller
    {
        public MyBotDB DB { get; set; }

        public MessageController()
        {
            this.DB = new MyBotDB();
        }

        // GET: Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            return View(new Message { BotID = bot.BotID, Bot = bot });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string id, Message message)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                var nextOrder = bot.Messages
                    .DefaultIfEmpty(new Message())
                    .Max(m => m.Order) + 1;
                bot.Messages.Add(new Message
                {
                    Text = message.Text,
                    Order = nextOrder
                });
                await this.DB.SaveChangesAsync();
                return RedirectToAction("Details", "Bot", new { id });
            }
            else
            {
                return View(new Message { BotID = bot.BotID, Bot = bot });
            }
        }

        [HttpGet]
        public ActionResult Edit(string id, int messageID)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var message = bot.Messages.FirstOrDefault(m => m.MessageID == messageID);
            if (message == null) return HttpNotFound();
            return View(message);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, int messageID, Message message)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var messageToEdit = bot.Messages.FirstOrDefault(m => m.MessageID == messageID);
            if (messageToEdit == null) return HttpNotFound();

            if (ModelState.IsValid)
            {
                messageToEdit.Text = message.Text;
                await this.DB.SaveChangesAsync();
                return RedirectToAction("Details", "Bot", new { id });
            }
            else
            {
                return View(message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Archive(string id, int messageID)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var messages = bot.Messages.ToList();
            var messageToArchive = messages.FirstOrDefault(m => m.MessageID == messageID);
            if (messageToArchive == null) return HttpNotFound();

            messageToArchive.IsArchived = true;

            await this.DB.SaveChangesAsync();

            return new EmptyResult();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id, int messageID)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var messages = bot.Messages.ToList();
            var messageToDelete = messages.FirstOrDefault(m => m.MessageID == messageID);
            if (messageToDelete == null) return HttpNotFound();

            this.DB.Messages.Remove(messageToDelete);

            // Renumber 'Order' property
            messages
                .Where(m => m.MessageID != messageToDelete.MessageID)
                .OrderBy(m => m.Order)
                .Select((m, i) => new { Message = m, NewOrder = i + 1 })
                .ToList()
                .ForEach(a => a.Message.Order = a.NewOrder);

            await this.DB.SaveChangesAsync();

            return new EmptyResult();
        }

        [HttpPost]
        public Task<ActionResult> Up(string id, int messageID)
        {
            return MoveUpOrDown(id, messageID, direction: -1);
        }

        [HttpPost]
        public Task<ActionResult> Down(string id, int messageID)
        {
            return MoveUpOrDown(id, messageID, direction: +1);
        }

        private async Task<ActionResult> MoveUpOrDown(string id, int messageID, int direction)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var messages = bot.Messages.ToArray();
            var message = messages.FirstOrDefault(m => m.MessageID == messageID);
            if (message == null) return HttpNotFound();

            var orgOrder = message.Order;
            var newOrder = message.Order + direction;
            var messageToReplace = messages.FirstOrDefault(m => m.Order == newOrder);
            if (messageToReplace == null) return Json(new { moved = false });

            message.Order = newOrder;
            messageToReplace.Order = orgOrder;
            await this.DB.SaveChangesAsync();

            return Json(new { moved = true });
        }
    }
}