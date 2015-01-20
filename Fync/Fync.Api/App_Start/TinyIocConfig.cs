using Fync.Common.Libraries;
using TinyIoC;

namespace Fync.Api.App_Start
{
    internal static class TinyIocConfig
    {
        public static void Configure()
        {
            var container = new TinyIoCContainer();
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new TinyIocApiResolver(container);

            Registrations.Register(container);
            Data.Registrations.Register(container);
            Service.Registrations.Register(container);
        }
    }
}
