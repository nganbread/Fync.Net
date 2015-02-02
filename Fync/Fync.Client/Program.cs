using Fync.Client.DataBase;
using Fync.Client.Dispatcher;
using Fync.Client.DispatchTasks;
using Fync.Client.HelperServices;
using Fync.Client.Web;
using Fync.Common;
using TinyIoC;

namespace Fync.Client
{
    class Program
    {
        private static void Main(string[] args)
        {
            var container = new TinyIoCContainer();
            Registrations.Register(container);
            container.Register<ILocalDatabase, LocalDatabase>().AsSingleton();
            container.Register<IHttpClient, HttpClientWrapper>().AsSingleton();
            container.Register<IHasher, CachedSha256Hasher>().AsSingleton();
            container.Register<IDirectoryHelper, DirectoryHelper>().AsSingleton();
            container.Register<IFileHelper, FileHelper>().AsSingleton();
            container.Register<IClientConfiguration, ClientConfiguration>().AsSingleton();
            container.Register<IDispatcher, Dispatcher.Dispatcher>().AsSingleton();
            container.Register<Application>();

            container.Register<FileSyncDispatchTask>().AsMultiInstance();
            container.Register<FolderSyncDispatchTask>().AsMultiInstance();
            container.Register<RootFolderSyncDispatchTask>().AsSingleton();
            
            container.Register<IDispatchFactory, DispatchFactory>().AsSingleton();
            
            container.Resolve<Application>().Start();
            container.Resolve<ILocalDatabase>().Dispose();
        }

    }
}
