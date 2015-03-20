using TinyIoC;

namespace Fync.Common
{
    public class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<IConfiguration, Configuration>();
            container.Register<IHasher, Sha256Hasher>().AsSingleton();
            container.Register(typeof(IFactory<>), typeof(Factory<>)).AsMultiInstance();
        }
    }
}
