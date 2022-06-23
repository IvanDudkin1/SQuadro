using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQuadro.Models
{
    public static class MvcHelper
    {
        public static string GetFullUrl(string actionName, string controllerName, object routeValues = null)
        {
            var request = HttpContext.Current.Request;
            var urlHelper = new UrlHelper(request.RequestContext);
            return urlHelper.Action(actionName, controllerName, routeValues, request.Url.Scheme);
        }
    }
}