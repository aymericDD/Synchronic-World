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
    public class IsAdmin : ActionFilterAttribute
    {
        private DataEntities db = new DataEntities();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            // Get User object in session
            User user = (User)HttpContext.Current.Session["user"];

            // If user is not admin return it to home page
            if (user != null)
            {
                user = (User)db.UserTable.First(p => p.UserEmail == user.UserEmail);
                db.Entry(user).Reload();
                HttpContext.Current.Session["user"] = user;

                if (user.UserRoleId != 1)
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary{{ "controller", "Home" },
                                          { "action", "Index" }
                                         });
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary{{ "controller", "Users" },
                                          { "action", "login" }
                                         });
            }

            base.OnActionExecuting(filterContext);
        }


    }
}