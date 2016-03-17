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
                    .Where(m => !m.IsArchived)
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

            // Renumber 'Order' property
            messages
                .Where(m => !m.IsArchived)
                .OrderBy(m => m.Order)
                .Select((m, i) => new { Message = m, NewOrder = i + 1 })
                .ToList()
                .ForEach(a => a.Message.Order = a.NewOrder);

            await this.DB.SaveChangesAsync();

            return new EmptyResult();
        }

        [HttpPost]
        public async Task<ActionResult> Restore(string id, int messageID)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            var messages = bot.Messages.ToList();
            var messageToRestore = messages.FirstOrDefault(m => m.MessageID == messageID);
            if (messageToRestore == null) return HttpNotFound();

            var nextOrder = messages
                .Where(m => !m.IsArchived)
                .DefaultIfEmpty(new Message())
                .Max(m => m.Order) + 1;
            messageToRestore.Order = nextOrder;

            messageToRestore.IsArchived = false;

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

            await this.DB.SaveChangesAsync();

            return new EmptyResult();
        }

        enum Direction
        {
            Up,
            Down
        }

        [HttpPost]
        public Task<ActionResult> Up(string id, int messageID)
        {
            return MoveUpOrDown(id, messageID, Direction.Up);
        }

        [HttpPost]
        public Task<ActionResult> Down(string id, int messageID)
        {
            return MoveUpOrDown(id, messageID, Direction.Down);
        }

        private async Task<ActionResult> MoveUpOrDown(string id, int messageID, Direction direction)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();

            var orderedAvailableMessages = bot.Messages.Where(m => !m.IsArchived).OrderBy(m => m.Order).ToArray();
            var message = orderedAvailableMessages.FirstOrDefault(m => m.MessageID == messageID);
            if (message == null) return HttpNotFound();

            var messageToReplace = direction == Direction.Up
                ? orderedAvailableMessages.LastOrDefault(m => m.Order < message.Order)
                : orderedAvailableMessages.FirstOrDefault(m => m.Order > message.Order);
            if (messageToReplace == null) return Json(new { moved = false });

            var currentOrder = message.Order;
            message.Order = messageToReplace.Order;
            messageToReplace.Order = currentOrder;
            await this.DB.SaveChangesAsync();

            return Json(new { moved = true });
        }
    }
}