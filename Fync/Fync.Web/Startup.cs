using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fync.Web.Startup /*Needs to be fully qualified*/))]
namespace Fync.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
