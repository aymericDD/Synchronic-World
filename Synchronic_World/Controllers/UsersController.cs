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

namespace Synchronic_World.Controllers
{
    public class UsersController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: Users
        public ActionResult Index()
        {
            var userTable = db.UserTable.Include(u => u.RoleUserTable);
            return View(userTable.ToList());
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserEmail,UserPassword")] User user)
        {
            // If user email or user password are empty return a error
            if (user.UserEmail == null || user.UserPassword == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Check if user is valide and if is valide add user id in session 
            User connectedUser = this.getUserByLoginAndPassword(user);
            if (connectedUser != null)
            {
                return RedirectToAction("Details", new { id = connectedUser.UserId });
            }

            // If user doesn't exist return error
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.UserTable.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserId,UserName,UserEmail,UserPassword,JoinDate,UserRoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserRoleId = 3;
                user.JoinDate = DateTime.Now;
                db.UserTable.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.UserTable.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,UserEmail,UserPassword,JoinDate,UserRoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.UserTable.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.UserTable.Find(id);
            db.UserTable.Remove(user);
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

        // Get user by login and password. If user don't exist return null
        private User getUserByLoginAndPassword(User user)
        {
            foreach (User item in db.UserTable.Where(p => p.UserEmail == user.UserEmail && p.UserPassword == user.UserPassword))
            {
                Session["id"] = item.UserId;

                User nUser = new User();

                nUser.UserId = item.UserId;
                nUser.UserName = item.UserName;
                nUser.UserPassword = item.UserPassword;
                nUser.UserRoleId = item.UserRoleId;
                nUser.UserEmail = item.UserEmail;
                nUser.JoinDate = item.JoinDate;

                return nUser;
            }

            return null;
        }
    }
}
