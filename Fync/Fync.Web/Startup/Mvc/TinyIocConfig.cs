using TinyIoC;

namespace Fync.Web
{
    internal static class TinyIocConfig
    {
        public static void Configure(TinyIoCContainer container)
        {
            Registrations.Register(container);
            Common.Registrations.Register(container);
            Data.Registrations.Register(container);
            Service.Registrations.Register(container);
        }
    }
}
