using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SchematBlokowy
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
#if !DEBUG
            MainDatabaseContext.MigrateData();
#endif
        }

        protected void Session_End(Object sender, EventArgs E)
        {
            Session.Clear();
        }
    }
}
