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
            var bot = GetBot(id);
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


        // GET: Bot/Edit/foo
        public ActionResult Edit(string id)
        {
            var bot = GetBot(id);
            if (bot == null) return HttpNotFound();
            return View(bot);
        }

        // POST: Bot/Edit/foo
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Bot model)
        {
            var bot = GetBot(id);
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
        public ActionResult TweetAsTheBot(string id, string text)
        {
            var bot = GetBot(id);
            if (bot == null) return HttpNotFound();

            var twitterAuthOpt = JsonConvert.DeserializeObject<TwitterAuthenticationOptions>(AppSettings.Key.Twitter);
            var token = CoreTweet.Tokens.Create(
                twitterAuthOpt.ConsumerKey,
                twitterAuthOpt.ConsumerSecret,
                bot.AccessToken, bot.AccessTokenSecret);
            token.Statuses.Update(status => text);

            return new EmptyResult();
        }

        private Bot GetBot(string botID)
        {
            var masterID = this.User.Identity.Name;
            var bot = this.DB.Bots
                .Where(b => b.BotID == botID)
                .FirstOrDefault(b => b.BotMasters.Any(master => master.MasterID == masterID));
            return bot;
        }
    }
}
