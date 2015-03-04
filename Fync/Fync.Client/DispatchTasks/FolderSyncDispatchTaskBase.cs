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
    internal abstract class FolderSyncDispatchTaskBase : IDispatchTask
    {
        protected internal DirectoryInfo _localFolder;
        protected internal FolderWithChildren ServerFolder;
        protected readonly IDispatchFactory _dispatchFactory;
        protected readonly IDispatcher _dispatcher;
        protected readonly IHttpClient _httpClient;

        protected FolderSyncDispatchTaskBase(DirectoryInfo localFolder, FolderWithChildren serverFolder, IDispatchFactory dispatchFactory, IDispatcher dispatcher, IHttpClient httpClient)
        {
            _localFolder = localFolder;
            ServerFolder = serverFolder;
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

        protected async Task SyncFiles()
        {
            //TODO: dont need to do this if we just made the server folder
            var serverFiles = await _httpClient.GetAsync<IList<SymbolicFile>>("{0}/SymbolicFile", ServerFolder.Id);
            foreach (var serverFile in serverFiles)
            {
                _dispatcher.Enqueue(_dispatchFactory.FileSync(ServerFolder, _localFolder.CreateFileInfo(serverFile.Name), serverFile));
            }

            foreach (var fileInfo in _localFolder.GetFiles().ToList())
            {
                var serverFile = serverFiles.SingleOrDefault(x => x.Name.Equals(fileInfo.Name, StringComparison.InvariantCultureIgnoreCase));
                _dispatcher.Enqueue(_dispatchFactory.FileSync(ServerFolder, fileInfo, serverFile));
            }
        }

        protected void SyncSubFolders()
        {
            var tasks = new List<IDispatchTask>();

            foreach (var subFolder in ServerFolder.SubFolders)
            {
                var task = _dispatchFactory.FolderSync(_localFolder.CreateSubdirectoryInfo(subFolder.Name), ServerFolder, subFolder);
                tasks.Add(task);
            }

            foreach (var subFolder in _localFolder.GetDirectories().ToList())
            {
                var serverSubFolder =  ServerFolder.SubFolders.SingleOrDefault(x => x.Name.Equals(subFolder.Name, StringComparison.InvariantCultureIgnoreCase));
                var task = _dispatchFactory.FolderSync(subFolder, ServerFolder, serverSubFolder);
                tasks.Add(task);
            }

            _dispatcher.Enqueue(tasks);
        }

        public abstract Task PerformAsync();
    }
}