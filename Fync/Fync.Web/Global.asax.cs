using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TinyIoC;

namespace Fync.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var container = new TinyIoCContainer();
            TinyIocConfig.Configure(container);
            
            DependencyResolver.SetResolver(container.Resolve<IDependencyResolver>());

            container.Resolve<IFilterConfig>().RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
