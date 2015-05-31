using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Synchronic_World;
using Synchronic_World.Models;
using Synchronic_World.App_Start;

namespace Synchronic_World.Controllers
{
    [IsLogginIn]
    public class ContributionEventsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: ContributionEvents
        public ActionResult Index()
        {
            User me = this.getCurrentUser();

            var contributionEvents = me.Contributions;
            return View(contributionEvents.ToList());
        }

        // GET: ContributionEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User me = this.getCurrentUser();
            ContributionEvent contributionEvent = me.Contributions.FirstOrDefault(c => c.ContributionEventId == id);
            if (contributionEvent == null)
            {
                return HttpNotFound();
            }
            return View(contributionEvent);
        }

        // GET: ContributionEvents/Create
        public ActionResult Create()
        {
            User me = this.getCurrentUser();

            IEnumerable<Event> LEvents = me.Events;

            ViewBag.EventId = new SelectList(LEvents, "EventId", "Title");
            return View();
        }

        // POST: ContributionEvents/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContributionEventId,ContributionEventName,ContributionEventQuantity,TypeContributionEvent,EventId")] ContributionEvent contributionEvent)
        {

            User me = this.getCurrentUser();

            if (ModelState.IsValid)
            {
                me.Contributions.Add(contributionEvent);
                db.ContributionEvents.Add(contributionEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            IEnumerable<Event> LEvents = me.Events;

            ViewBag.EventId = new SelectList(LEvents, "EventId", "Title");
            return View(contributionEvent);
        }

        // GET: ContributionEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User me = this.getCurrentUser();
            ContributionEvent contributionEvent = me.Contributions.FirstOrDefault(c => c.ContributionEventId == id);
            if (contributionEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(me.Events, "EventId", "Title", contributionEvent.EventId);
            return View(contributionEvent);
        }

        // POST: ContributionEvents/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContributionEventId,ContributionEventName,ContributionEventQuantity,TypeContributionEvent,EventId")] ContributionEvent contributionEvent)
        {
            User me = this.getCurrentUser();

            if (ModelState.IsValid)
            {
                db.Entry(contributionEvent).State = EntityState.Modified;
                me.Contributions.Add(contributionEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(me.Events, "EventId", "Title", contributionEvent.EventId);
            return View(contributionEvent);
        }

        // GET: ContributionEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User me = this.getCurrentUser();
            ContributionEvent contributionEvent = me.Contributions.FirstOrDefault(c => c.ContributionEventId == id);
            if (contributionEvent == null)
            {
                return HttpNotFound();
            }
            return View(contributionEvent);
        }

        // POST: ContributionEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User me = this.getCurrentUser();
            ContributionEvent contributionEvent = me.Contributions.FirstOrDefault(c => c.ContributionEventId == id);
            db.ContributionEvents.Remove(contributionEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public User getCurrentUser()
        {
            User user = (User)HttpContext.Session["user"];

            return db.UserTable.Find(user.UserId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
