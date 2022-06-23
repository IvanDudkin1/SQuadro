using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;
using System.Threading;
using System.IO;
using SQuadro;
using System.Web.Routing;
using System.Data.Entity;

public static class CommonExtensions
{
    public static string ToFormat(this string pendingString, params object[] args)
    {
        return string.Format(pendingString, args);
    }

    public static string Left(this string pendingString, int length)
    {
        if (length > pendingString.Length)
            length = pendingString.Length;
        return pendingString.Substring(0, length);
    }

    public static IEnumerable<Guid> ToGuidArray(this string value, char delimeter = ',')
    {
        Guid tmpID = Guid.Empty;
        return value.Split(delimeter).Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID);
    }

    public static string JoinToString(this IEnumerable<Guid> values, char delimeter = ',')
    {
        if (values.Any())
            return values.Select(item => item.ToString()).Aggregate((item1, item2) => item1 + delimeter + item2);
        else
            return String.Empty;
    }

    public static bool IsInRole(this MembershipUser user, string role)
    {
        RolePrincipal rolePrincipal = new RolePrincipal(new GenericIdentity(user.UserName));
        return rolePrincipal.IsInRole(role);
    }

    public static DateTime PrevWorkingDay(this DateTime date)
    {
        date = date.AddDays(-1);
        while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            date = date.AddDays(-1);
        return date;
    }

    public static MvcHtmlString CalendarFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
    {
        var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        if (!attributes.ContainsKey("class"))
            attributes["class"] = "date-picker";
        else
            if (!((string)attributes["class"]).Contains("date-picker"))
                attributes["class"] += " date-picker";

        attributes["data-date-format"] = htmlHelper.ConvertDateFormat();

        var mvcHtmlString = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, attributes);
        var xDoc = XDocument.Parse(mvcHtmlString.ToHtmlString());
        var xElement = xDoc.Element("input");
        if (xElement != null)
        {
            var valueAttribute = xElement.Attribute("value");
            if (valueAttribute != null)
            {
                DateTime value;
                if (DateTime.TryParse(valueAttribute.Value, out value))
                {
                    if (value != new DateTime(1, 1, 1))
                        valueAttribute.Value = value.ToShortDateString();
                    else
                        valueAttribute.Value = string.Empty;
                }
                else
                    valueAttribute.Value = string.Empty;
            }
        }
        return new MvcHtmlString(xDoc.ToString());
    }

    public static MvcHtmlString IntegerTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
    {
        var mvcHtmlString = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, htmlAttributes ?? new { @class = "text-box single-line numerik" });
        var xDoc = XDocument.Parse(mvcHtmlString.ToHtmlString());
        var xElement = xDoc.Element("input");
        if (xElement != null)
        {
            var valueAttribute = xElement.Attribute("value");
            if (valueAttribute != null)
            {
                int value;
                if (Int32.TryParse(valueAttribute.Value, out value))
                {
                    if (value != 0)
                        valueAttribute.Value = value.ToString();
                    else
                        valueAttribute.Value = string.Empty;
                }
                else
                    valueAttribute.Value = string.Empty;
            }
        }
        return new MvcHtmlString(xDoc.ToString());
    }

    public static string ConvertDateFormat(this HtmlHelper html)
    {
        return ConvertDateFormat(html, Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
    }

    public static string ConvertDateFormat(this HtmlHelper html, string format)
    {
        return format.Replace("MMM", "m").Replace("MM", "mm");
    }

    public static string PartialViewToString(this Controller controller)
    {
        return controller.PartialViewToString(null, null);
    }

    public static string RenderPartialViewToString(this Controller controller, string viewName)
    {
        return controller.PartialViewToString(viewName, null);
    }

    public static string RenderPartialViewToString(this Controller controller, object model)
    {
        return controller.PartialViewToString(null, model);
    }

    public static string PartialViewToString(this Controller controller, string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
        {
            viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
        }

        controller.ViewData.Model = model;

        using (StringWriter stringWriter = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, stringWriter);
            viewResult.View.Render(viewContext, stringWriter);
            return stringWriter.GetStringBuilder().ToString();
        }
    }

    public static bool HasPendingChanges(this EntityContext context)
    {
        return context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified).Any();
    }

    public static double JSMilliseconds(this DateTime value)
    {
        return value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    public static double JSMilliseconds(this DateTime? value)
    {
        return JSMilliseconds(value ?? new DateTime(1970, 1, 1));
    }

    public static string GetMessage(this Exception exception)
    {
        Exception e = exception;
        while (e.InnerException != null)
            e = e.InnerException;
        return e.Message;
    }

    public static DateTimeOffset ToTimeZone(this DateTimeOffset dateTimeOffset, string timeZoneId)
    {
        TimeZoneInfo tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        if (tzInfo == null)
            throw new InvalidOperationException("Wrong TimeZone Id {0}".ToFormat(timeZoneId));

        return TimeZoneInfo.ConvertTime(dateTimeOffset, tzInfo);
    }
}