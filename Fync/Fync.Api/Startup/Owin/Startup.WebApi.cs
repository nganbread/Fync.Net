using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using Fync.Api.App_Start;
using Microsoft.Owin.Cors;
using Owin;

namespace Fync.Api
{
    internal partial class Startup
    {
        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
#if DEBUG
            var corsPolicy = new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUSH, DELETE");
            // Enable CORS for ASP.NET Identity
            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = request =>
                        request.Path.Value == "/token" ?
                        corsPolicy.GetCorsPolicyAsync(null, CancellationToken.None) :
                        Task.FromResult<CorsPolicy>(null)
                }
            });

            // Enable CORS for Web API
            config.EnableCors(corsPolicy);
            config.EnableCors(corsPolicy);
#endif
            TinyIocConfig.Configure(config);
            RouteConfig.Configure(config);

            app.UseWebApi(config);
        }
    }
}