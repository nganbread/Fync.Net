using System.Web.Http;
using Fync.Api.App_Start;
using Owin;

namespace Fync.Api
{
    internal partial class Startup
    {
        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            
            TinyIocConfig.Configure(config);
            RouteConfig.Configure(config);

            app.UseWebApi(config);
        }
    }
}