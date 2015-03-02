using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Dispatcher;
using Fync.Client.Extensions;
using Fync.Client.Web;
using Fync.Common.Models;

namespace Fync.Client.DispatchTasks
{
    internal abstract class FolderSyncDispatchTaskBase : DispatchTaskBase
    {
        protected internal DirectoryInfo _localFolder;
        protected internal FolderWithChildren ServerFolderWithChildren;
        protected readonly IDispatchFactory _dispatchFactory;
        protected readonly IDispatcher _dispatcher;
        protected readonly IHttpClient _httpClient;

        protected FolderSyncDispatchTaskBase(DirectoryInfo localFolder, FolderWithChildren serverFolderWithChildren, IDispatchFactory dispatchFactory, IDispatcher dispatcher, IHttpClient httpClient)
        {
            _localFolder = localFolder;
            ServerFolderWithChildren = serverFolderWithChildren;
            _dispatchFactory = dispatchFactory;
            _dispatcher = dispatcher;
            _httpClient = httpClient;
        }

        protected FolderSyncDispatchTaskBase(IDispatchFactory dispatchFactory, IDispatcher dispatcher, IHttpClient httpClient)
        {
            _dispatchFactory = dispatchFactory;
            _dispatcher = dispatcher;
            _httpClient = httpClient;
        }

        public override int Priority
        {
            get { return 2; }
        }


        protected async Task SyncFiles()
        {
            //TODO: dont need to do this if we just made the server folder
            var serverFiles = await _httpClient.GetAsync<IList<SymbolicFile>>("{0}/SymbolicFile", ServerFolderWithChildren.Id);
            foreach (var serverFile in serverFiles)
            {
                _dispatcher.Queue(_dispatchFactory.FileSync(ServerFolderWithChildren, _localFolder.CreateFileInfo(serverFile.Name), serverFile));
            }

            foreach (var fileInfo in _localFolder.GetFiles().ToList())
            {
                var serverFile = serverFiles.SingleOrDefault(x => x.Name.Equals(fileInfo.Name, StringComparison.InvariantCultureIgnoreCase));
                _dispatcher.Queue(_dispatchFactory.FileSync(ServerFolderWithChildren, fileInfo, serverFile));
            }
        }

        protected void SyncSubFolders()
        {
            var tasks = new List<IDispatchTask>();

            foreach (var subFolder in ServerFolderWithChildren.SubFolders)
            {
                var task = _dispatchFactory.FolderSync(_localFolder.CreateSubdirectoryInfo(subFolder.Name), ServerFolderWithChildren, subFolder);
                tasks.Add(task);
            }

            foreach (var subFolder in _localFolder.GetDirectories().ToList())
            {
                var serverSubFolder =  ServerFolderWithChildren.SubFolders.SingleOrDefault(x => x.Name.Equals(subFolder.Name, StringComparison.InvariantCultureIgnoreCase));
                var task = _dispatchFactory.FolderSync(subFolder, ServerFolderWithChildren, serverSubFolder);
                tasks.Add(task);
            }

            _dispatcher.Queue(tasks);
        }
    }
}