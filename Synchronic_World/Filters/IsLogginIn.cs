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
    public class IsLogginIn : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get object User in session
            User user = (User)HttpContext.Current.Session["user"];

            // If user exist, it is connected else return it to login page
            if (user == null)
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