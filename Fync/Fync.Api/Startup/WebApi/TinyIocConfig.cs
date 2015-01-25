using System.Web.Http;
using Fync.Common.Libraries;
using TinyIoC;

namespace Fync.Api.App_Start
{
    internal static class TinyIocConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            var container = new TinyIoCContainer();

            Registrations.Register(container);
            Common.Registrations.Register(container);
            Data.Registrations.Register(container);
            Service.Registrations.Register(container);

            config.DependencyResolver = new TinyIocApiResolver(container);
        }
    }
}
