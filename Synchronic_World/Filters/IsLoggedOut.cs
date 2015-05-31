using Synchronic_World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using System.Web.Security;

namespace Synchronic_World.App_Start
{
    public class IsLoggedOut : ActionFilterAttribute
    {
        private DataEntities db = new DataEntities();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            // Get User object in session
            User user = (User)HttpContext.Current.Session["user"];

            // If user exist return it to home page
            if (user != null)
            {
                user = (User)db.UserTable.First(p => p.UserEmail == user.UserEmail);
                db.Entry(user).Reload();
                HttpContext.Current.Session["user"] = user;

                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary{{ "controller", "Home" },
                                          { "action", "Index" }
                                         });
            }

            base.OnActionExecuting(filterContext);
        }


    }
}