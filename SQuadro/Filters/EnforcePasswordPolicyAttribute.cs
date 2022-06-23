using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SQuadro.Filters
{
    public class EnforcePasswordPolicyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid personID;
            if (Guid.TryParse(HttpContext.Current.User.Identity.Name, out personID))
            {
                User user = EntityContext.Current.Users.FirstOrDefault(u => u.ID == personID);
                if (user != null && user.ForceChangePassword)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "ChangePassword" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}