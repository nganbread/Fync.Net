using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace Fync.Api.App_Start
{
    public static class RouteConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Fync",
                routeTemplate: "Fync/{*pathComponents}",
                defaults: new { controller = "Fync" });

            config.Routes.MapHttpRoute(
                name: "Folder",
                routeTemplate: "Folder/{*pathComponents}",
                defaults: new { controller = "Folder" });

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{controller}");

            config.Routes.MapHttpRoute(
                name: "FolderContext",
                routeTemplate: "{folderId}/{controller}");
        }
    }
}
