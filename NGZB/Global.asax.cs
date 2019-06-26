using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NGZB.Controllers;

namespace NGZB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BundleTable.EnableOptimizations = SysController.__EnabelZipCssAndJs();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            Session["loginUserCode"] = null;
        }
    }
}