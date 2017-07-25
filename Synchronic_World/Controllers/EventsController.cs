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
    public class EventsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: Events
        [IsAdmin]
        public ActionResult Index()
        {
            var eventTable = db.EventTable.Include(u => u.UserTable);
            return View(eventTable.ToList());
        }

        // GET: Events/DetailsEvent/5
        [IsLogginIn]
        public ActionResult DetailsEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event @event = this.getEventById(id);

            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/NewEvent
        [IsLogginIn]
        public ActionResult NewEvent()
        {
            return View();
        }

        // POST: Events/NewEvent
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [IsLogginIn]
        [ValidateAntiForgeryToken]
        public ActionResult NewEvent([Bind(Include = "EventId,EndDate,StartDate,Title,Content,Address,TypeEvent")] Event @event)
        {
            if (ModelState.IsValid)
            {
                User user = (User)HttpContext.Session["user"];
                @event.OwnerId = user.UserId;
                @event.StatusEvent = Synchronic_World.Models.Type.StatusEvent.Pending;
                db.EventTable.Add(@event);
                db.SaveChanges();
                return RedirectToAction("DetailsEvent", new { id = @event.EventId });
            }

            ViewBag.OwnerId = new SelectList(db.UserTable, "UserId", "UserName", @event.OwnerId);
            return View(@event);
        }

        // GET: Events/MyEvents
        [IsLogginIn]
        public ActionResult MyEvents()
        {
            User user = (User)HttpContext.Session["user"];
            User me = db.UserTable.Find(user.UserId);
            ICollection<Event> LEvents = me.Events;
            return View(LEvents);
        }


        // GET: Events/InvitedEvents
        [IsLogginIn]
        public ActionResult InvitedEvents()
        {
            User user = (User)HttpContext.Session["user"];
            User me = db.UserTable.Find(user.UserId);
            ICollection<Event> LEvents = me.ParticpationEvents;
            return View(LEvents);
        }

        [IsLogginIn]
        public ActionResult AddUser(int? id)
        {
            int eventId = int.Parse(this.Request.Params["p_Event"]);

            User user = (User)HttpContext.Session["user"];
            User me = db.UserTable.Find(user.UserId);

            User friend = me.friends.FirstOrDefault(u => u.UserId == id);
            Event currentEvent = db.EventTable.Find(eventId);

            if (friend != null && currentEvent != null)
            {
                currentEvent.Participants.Add(friend);
                friend.ParticpationEvents.Add(currentEvent);
                db.SaveChanges();
            }

            return RedirectToAction("EditEvent", new { id = currentEvent.EventId });
        }

        // GET: Events/Create
        [IsAdmin]
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.UserTable, "UserId", "UserName");
            return View();
        }

        // POST: Events/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [IsAdmin]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,EndDate,StartDate,Title,Content,Address,OwnerId,StatusEvent,TypeEvent")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.EventTable.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.UserTable, "UserId", "UserName", @event.OwnerId);
            return View(@event);
        }

        // GET: Events/Details/5
        [IsLogginIn]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.EventTable.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/EditEvent/5
        [IsLogginIn]
        public ActionResult EditEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = this.getEventById(id);

            if (@event == null)
            {
                return HttpNotFound();
            }

            ViewBag.OwnerId = new SelectList(db.UserTable, "UserId", "UserName", @event.OwnerId);
            return View(@event);
        }

        // POST: Events/EditEvent/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [IsLogginIn]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent([Bind(Include = "EventId,EndDate,StartDate,Title,Content,Address,OwnerId,StatusEvent,TypeEvent")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.UserTable, "UserId", "UserName", @event.OwnerId);
            return View(@event);
        }

        // GET: Events/Edit/5
        [IsAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.EventTable.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(db.UserTable, "UserId", "UserName", @event.OwnerId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [IsAdmin]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,EndDate,StartDate,Title,Content,Address,OwnerId,StatusEvent,TypeEvent")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.UserTable, "UserId", "UserName", @event.OwnerId);
            return View(@event);
        }

        // GET: Events/DeleteEvent/5
        [IsLogginIn]
        public ActionResult DeleteEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = this.getEventById(id);

            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/DeleteEvent/5
        [HttpPost, ActionName("DeleteEvent")]
        [IsLogginIn]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEventConfirmed(int id)
        {
            Event @event = this.getEventById(id);
            db.EventTable.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public Event getEventById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            User user = (User)HttpContext.Session["user"];
            User me = db.UserTable.Find(user.UserId);

            return me.Events.FirstOrDefault(p => p.EventId == id);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.EventTable.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.EventTable.Find(id);
            db.EventTable.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
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
