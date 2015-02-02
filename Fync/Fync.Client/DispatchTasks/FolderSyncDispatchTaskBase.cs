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
        protected internal Folder _serverFolder;
        protected readonly IDispatchFactory _dispatchFactory;
        protected readonly IDispatcher _dispatcher;
        protected readonly IHttpClient _httpClient;

        protected FolderSyncDispatchTaskBase(DirectoryInfo localFolder, Folder serverFolder, IDispatchFactory dispatchFactory, IDispatcher dispatcher, IHttpClient httpClient)
        {
            _localFolder = localFolder;
            _serverFolder = serverFolder;
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
            var serverFiles = await _httpClient.GetAsync<IList<SymbolicFile>>("{0}/SymbolicFile", _serverFolder.Id);
            foreach (var serverFile in serverFiles)
            {
                _dispatcher.Add(_dispatchFactory.FileSync(_serverFolder, _localFolder.CreateFileInfo(serverFile.Name), serverFile));
            }

            foreach (var fileInfo in _localFolder.GetFiles().ToList())
            {
                var serverFile = serverFiles.SingleOrDefault(x => x.Name.Equals(fileInfo.Name, StringComparison.InvariantCultureIgnoreCase));
                _dispatcher.Add(_dispatchFactory.FileSync(_serverFolder, fileInfo, serverFile));
            }
        }

        protected void SyncSubFolders()
        {
            var tasks = new List<IDispatchTask>();

            foreach (var subFolder in _serverFolder.SubFolders)
            {
                var task = _dispatchFactory.FolderSync(_localFolder.CreateSubdirectoryInfo(subFolder.Name), _serverFolder, subFolder);
                tasks.Add(task);
            }

            foreach (var subFolder in _localFolder.GetDirectories().ToList())
            {
                var serverSubFolder =  _serverFolder.SubFolders.SingleOrDefault(x => x.Name.Equals(subFolder.Name, StringComparison.InvariantCultureIgnoreCase));
                var task = _dispatchFactory.FolderSync(subFolder, _serverFolder, serverSubFolder);
                tasks.Add(task);
            }

            _dispatcher.Add(tasks);
        }
    }
}