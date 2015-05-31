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
using System.Web.Security;
using Synchronic_World.App_Start;

namespace Synchronic_World.Controllers
{
    public class UsersController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: Users
        [IsAdmin]
        public ActionResult Index()
        {
            var userTable = db.UserTable.Include(u => u.RoleUserTable);
            return View(userTable.ToList());
        }

        // GET: Users/Login
        [IsLoggedOut]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [IsLoggedOut]
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
                // Connecte user
                return this.connecteUser(connectedUser);
            }

            ModelState.AddModelError("ErrorMessage", "Login or password");

            // If user doesn't exist return error
            return View(user);
        }

        // GET: Users/Logout
        public ActionResult Logout()
        {
            // Remove user in session
            return this.disconnecteUser();
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
            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // GET: Users/Register
        [IsLoggedOut]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [IsLoggedOut]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserId,UserName,UserEmail,UserPassword,JoinDate")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserRoleId = 3;
                user.JoinDate = DateTime.Now;
                db.UserTable.Add(user);
                db.SaveChanges();
                return this.connecteUser(user);
            }

            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // GET: Users/Create
        [IsAdmin]
        public ActionResult Create()
        {
            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role");
            return View();
        }

        // GET: Users/EditProfile
        [IsLogginIn]
        public ActionResult EditProfile()
        {
            User user = (User)HttpContext.Session["user"];
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // POST: Users/EditProfile
        [HttpPost]
        [IsLogginIn]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "UserId,UserName,UserEmail,UserPassword,JoinDate,UserRoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                User ConnectedUser = (User)HttpContext.Session["user"];
                ConnectedUser.UserName = user.UserName;
                ConnectedUser.UserEmail = user.UserEmail;
                return View(ConnectedUser);
            }
            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // GET: Users/ChangePassword
        [IsLogginIn]
        public ActionResult ChangePassword()
        {
            User user = (User)HttpContext.Session["user"];
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // POST: Users/ChangePassword
        [HttpPost]
        [IsLogginIn]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "UserId,UserName,UserEmail,UserPassword,JoinDate,UserRoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                User ConnectedUser = (User)HttpContext.Session["user"];
                ConnectedUser.UserPassword = user.UserPassword;
                return View(ConnectedUser);
            }
            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // POST: Users/Create
        [HttpPost]
        [IsAdmin]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,UserEmail,UserPassword,JoinDate,UserRoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.UserTable.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserRoleId = new SelectList(db.RoleUserTable, "RoleUserId", "Role", user.UserRoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        [IsAdmin]
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
        [IsAdmin]
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
        [IsAdmin]
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
        [IsAdmin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.UserTable.Find(id);
            db.UserTable.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [IsLogginIn]
        public ActionResult SearchFriend()
        {
            IEnumerable<User> LFriends = (IEnumerable<User>)db.UserTable.ToList<User>();
            if (LFriends.Count() > 0 && LFriends != null)
            {
                return View(LFriends);
            }
            return View();
        }

        [HttpPost]
        [IsLogginIn]
        public ActionResult SearchFriend(String userName)
        {
            IEnumerable<User> LFriends = (IEnumerable<User>)db.UserTable.Where(u => u.UserName.Contains(userName)).ToList<User>();
            if (LFriends.Count() > 0 && LFriends != null)
            {
                return View(LFriends);
            }
            return View();
        }

        [IsLogginIn]
        public ActionResult AddFriend(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User friend = db.UserTable.FirstOrDefault(p => p.UserId == id);

            User currentUser = (User)HttpContext.Session["User"];

            int id1 = currentUser.UserId;

            if (friend != null)
            {
                User me = db.UserTable.FirstOrDefault(u => u.UserId == id1);

                me.friends.Add(friend);

                friend.friends.Add(me);
                
                db.SaveChanges();
            }

            return RedirectToAction("SearchFriend");
        }

        public ActionResult Friends()
        {
            User currentUser = (User)HttpContext.Session["User"];
            User me = db.UserTable.FirstOrDefault(u => u.UserId == currentUser.UserId);
            IEnumerable<User> LFriends = me.friends;
            return View(LFriends);
        }

        [IsLogginIn]
        public ActionResult RemoveFriend(int? id)
        {
            return View();
        }

        [IsLogginIn]
        public ActionResult SearchUsers()
        {

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Add user object into session
        private RedirectToRouteResult connecteUser(User user)
        {
            HttpContext.Session.Add("user", user);
            User us = (User)HttpContext.Session["user"];
            return RedirectToAction("Details", new { id = user.UserId });
        }

        private RedirectToRouteResult disconnecteUser()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login");
        }

        // Get user by login and password. If user don't exist return null
        private User getUserByLoginAndPassword(User user)
        {
            // Check if user exist
            User userConnected = null;
            try
            {
                 userConnected  = db.UserTable.First(p => p.UserEmail == user.UserEmail && p.UserPassword == user.UserPassword);
            }
            catch (Exception)
            {
                Console.WriteLine("User not find");
            }

            if (userConnected != null)
            {
                return userConnected;
            }

            return null;

        }
    }
}
