using Fync.Common.Libraries;
using TinyIoC;

namespace Fync.Data
{
    public static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<IContext, Context>().AsPerRequestSingleton();
        }
    }
}