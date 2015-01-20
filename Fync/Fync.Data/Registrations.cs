using System.Data.Entity;
using Fync.Common.Libraries;
using TinyIoC;

namespace Fync.Data
{
    public static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<Context>().AsPerRequestSingleton();
            container.Register<IContext>((c, overloads) => c.Resolve<Context>());
            container.Register<DbContext>((c, overloads) => c.Resolve<Context>());
        }
    }
}