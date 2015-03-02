using System.Web.Mvc;
using System.Web.Routing;

namespace Fync.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //See Web.Config <UrlRouteHandler>
            routes.MapRoute(
                name: "Fync",
                url: "Fync/{*pathComponents}",
                defaults: new { controller = "Fync", action = "Index" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}