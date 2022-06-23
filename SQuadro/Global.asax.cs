using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SQuadro.ModelBinders;
using SQuadro.Models;

namespace SQuadro
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            System.Web.Mvc.ModelBinders.Binders.Add(typeof(Guid), new GuidModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DataTablesParam), new DataTablesModelBinder());
        }

        protected virtual void Application_BeginRequest()
        {
            EntityContext context = new EntityContext(HttpContext.Current);
            context.CommandTimeout = 600;
        }

        protected virtual void Application_EndRequest()
        {
            if (EntityContext.Current != null)
                EntityContext.Current.Dispose();
        }
    }
}