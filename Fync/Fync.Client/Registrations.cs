using Fync.Client.DataBase;
using Fync.Client.Dispatcher;
using Fync.Client.DispatchTasks;
using Fync.Client.HelperServices;
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
            container.Register<IDirectoryHelper, DirectoryHelper>().AsSingleton();
            container.Register<IFileHelper, FileHelper>().AsSingleton();
            container.Register<IDispatcher, Dispatcher.Dispatcher>().AsSingleton();
            container.Register<IFileMonitor, FileMonitor>().AsSingleton();
            container.Register<IFolderMonitor, FolderMonitor>().AsSingleton();
            container.Register<ISyncEngine, SyncEngine>();

            container.Register<FileSyncDispatchTask>().AsMultiInstance();
            container.Register<FolderSyncDispatchTask>().AsMultiInstance();
            container.Register<RootFolderSyncDispatchTask>().AsSingleton();
            
            container.Register<IDispatchFactory, DispatchFactory>().AsSingleton();
        }

    }
}
