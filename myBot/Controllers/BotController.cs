using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Twitter;
using myBot.Code;
using myBot.Models;
using Newtonsoft.Json;

namespace myBot.Controllers
{
    [Authorize]
    public class BotController : Controller
    {
        public MyBotDB DB { get; set; }

        public BotController()
        {
            this.DB = new MyBotDB();
        }

        // GET: Bot
        public ActionResult Index()
        {
            var myBots = this.DB
                .BotMasters
                .Where(m => m.MasterID == this.User.Identity.Name)
                .Select(m => m.Bot);
            return View(myBots);
        }

        // GET: Bot/Details/foo
        public ActionResult Details(string id)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            return View(bot);
        }

        // GET: Bot/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bot/Create
        [HttpPost]
        public ActionResult Create(Bot bot)
        {
            if (ModelState.IsValid)
            {
                return new ChallengeResult(
                    provider: "Twitter",
                    redirectUri: Url.Action("AuthBotCallback", "Bot"),
                    userId: bot.BotID);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> AuthBotCallback()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            var signInInfo = await authenticationManager.GetExternalLoginInfoAsync();
            var claims = signInInfo.ExternalIdentity.Claims;
            var botID = signInInfo.ExternalIdentity.Name;
            var accessToken = claims.ValueOf(CustomClaimTypes.Twitter.AccessToken);
            var accessTokenSecret = claims.ValueOf(CustomClaimTypes.Twitter.AccessTokenSecret);

            this.DB.Bots.Add(new Bot
            {
                BotID = botID,
                AccessToken = accessToken,
                AccessTokenSecret = accessTokenSecret
            });
            this.DB.BotMasters.Add(new BotMaster
            {
                BotID = botID,
                MasterID = this.User.Identity.Name
            });
            this.DB.SaveChanges();

            return RedirectToAction("Details", "Bot", new { id = botID });
        }

        [HttpPost]
        public async Task<ActionResult> ChangeEnable(string id, bool enabled)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            bot.Enabled = enabled;
            await this.DB.SaveChangesAsync();
            return new EmptyResult();
        }

        // GET: Bot/Edit/foo
        public ActionResult Edit(string id)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();
            return View(bot);
        }

        // POST: Bot/Edit/foo
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Bot model)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();

            if (ModelState.IsValid)
            {
                UpdateModel(
                    model: bot,
                    includeProperties: new[] { "TimeZone", "BeginTime", "EndTime", "Duration" });
                await this.DB.SaveChangesAsync();

                return RedirectToAction("Details", new { id });
            }
            else
            {
                return View(bot);
            }
        }

        // GET: Bot/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Bot/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> TweetAsTheBot(string id, string text)
        {
            var bot = this.DB.Bots.GetById(this.User, id);
            if (bot == null) return HttpNotFound();

            await bot.TweetAsync(text);

            return new EmptyResult();
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> TweetScheduledMessage(string utctime)
        {
            var alivedBots = this.DB.Bots.Include("Messages")
                .Where(bot => bot.Enabled)
                .Where(bot => bot.Messages.Any())
                .Where(bot => bot.BotMasters.Any())
                .ToArray();

            const int BUFF = 2;
            var utcNow = utctime == null ? DateTime.UtcNow : DateTime.Parse(utctime);
            var utcNowTime = new DateTime(1900, 1, 1, utcNow.Hour, utcNow.Minute, 0);
            var botsToTweet = alivedBots
                .Where(bot => bot.GetTweetTimingsUTC().Any(t => Math.Abs((t - utcNowTime).TotalMinutes) <= BUFF))
                .Select(bot => new { bot, message = bot.GetMessageToNextTweet() })
                .ToList();

            botsToTweet.ForEach(_ => _.message.AtLastTweeted = utcNow);
            var tweetTasks = botsToTweet.Select(_ => _.bot.TweetAsync(_.message.Text)).ToArray();
            await Task.WhenAll(tweetTasks);

            await this.DB.SaveChangesAsync();

            return new EmptyResult();
        }
    }
}
