using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fync.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            var configuration = TinyIocConfig.Configure();
            DependencyResolver.SetResolver(configuration);
            
        }
    }
}
