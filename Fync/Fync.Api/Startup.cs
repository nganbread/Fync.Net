using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fync.Api.Startup /*Needs to be fully qualified*/))]
namespace Fync.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureWebApi(app);
        }
    }
}
