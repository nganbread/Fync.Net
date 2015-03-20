using Fync.Client.Hash;
using Fync.Client.Monitor;
using Fync.Client.Traverser;
using Fync.Client.Visitors;
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
            container.Register<ISyncEngine, SyncEngine>();
            container.Register<IFileChangeDetector, FileChangeDetector>();
            container.Register<IFolderChangeDetector, FolderChangeDetector>();

            container.AutoRegister(DuplicateImplementationActions.RegisterMultiple, x => x.IsAssignableFrom(typeof(IStrategy<>)));
            container.AutoRegister(DuplicateImplementationActions.RegisterMultiple, x => x.IsAssignableFrom(typeof(ITraverser)));
        }
    }
}
