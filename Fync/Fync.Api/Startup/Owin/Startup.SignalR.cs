using Owin;

namespace Fync.Api
{
    internal partial class Startup
    {
        private void ConfigureSignalR(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}