using Fync.Common.Libraries;
using TinyIoC;

namespace Fync.Web
{
    internal static class TinyIocConfig
    {
        public static TinyIocMvcResolver Configure()
        {
            var container = new TinyIoCContainer();

            Registrations.Register(container);
            Common.Registrations.Register(container);
            Data.Registrations.Register(container);
            Service.Registrations.Register(container);

            return new TinyIocMvcResolver(container);
        }
    }
}
