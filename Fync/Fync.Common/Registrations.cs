using TinyIoC;

namespace Fync.Common
{
    public class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<IConfiguration, Configuration>();
        }
    }
}
