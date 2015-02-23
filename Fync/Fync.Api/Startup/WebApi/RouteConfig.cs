﻿using System.Web.Http;

namespace Fync.Api.App_Start
{
    public static class RouteConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{controller}");

            config.Routes.MapHttpRoute(
                name: "FolderContext",
                routeTemplate: "{folderId}/{controller}");


        }
    }
}
