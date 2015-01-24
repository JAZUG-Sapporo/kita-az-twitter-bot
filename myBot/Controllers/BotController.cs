using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myBot.Models;

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

        // GET: Bot/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Bot/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bot/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Bot/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Bot/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
    }
}
