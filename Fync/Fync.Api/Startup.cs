using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fync.Api.Startup /*Needs to be fully qualified*/))]
namespace Fync.Api
{
    internal partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureWebApi(app);
            ConfigureAuth(app);
            ConfigureSignalR(app);
        }
    }
}
