using Fync.Client.Hash;
using Fync.Client.Monitor;
using Fync.Client.Web;
using Fync.Common;
using TinyIoC;

namespace Fync.Client
{
    public static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<IHttpClient, HttpClientWrapper>().AsSingleton();
            container.Register<IHasher, CachedSha256Hasher>().AsSingleton();
            container.Register<IFileMonitor, FileMonitor>().AsSingleton();
            container.Register<IFolderMonitor, FolderMonitor>().AsSingleton();
            container.Register<ISyncEngine, SyncEngine>();

            container.Register<SyncEngine>();
        }

    }
}
