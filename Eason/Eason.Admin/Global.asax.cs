using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Eason.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
         
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ExpressMapperConfig.RegisterMapper();
            AutofacConfig.RegisterIOC();
        }
    }
}
