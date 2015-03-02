using System.Threading.Tasks;
using Fync.Client.Dispatcher;
using Fync.Client.Web;
using Fync.Common.Models;

namespace Fync.Client.DispatchTasks
{
    internal class RootFolderSyncDispatchTask : FolderSyncDispatchTaskBase
    {
        private readonly IClientConfiguration _clientConfiguration;

        public RootFolderSyncDispatchTask(IDispatchFactory dispatchFactory, IDispatcher dispatcher, IHttpClient httpClient, IClientConfiguration clientConfiguration)
            : base(dispatchFactory, dispatcher, httpClient)
        {
            _clientConfiguration = clientConfiguration;
        }

        public override async Task Perform()
        {
            await Initialise();
            SyncSubFolders();
            await SyncFiles();
        }

        private async Task Initialise()
        {
            ServerFolderWithChildren = await _httpClient.GetAsync<FolderWithChildren>("Folder");
            _localFolder = _clientConfiguration.BaseDirectory;
            _localFolder.Create();
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override int Priority
        {
            get { return 0; }
        }

        public override bool Equals(object obj)
        {
            return obj is RootFolderSyncDispatchTask;
        }
    }
}