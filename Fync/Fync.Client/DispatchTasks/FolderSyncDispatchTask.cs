using System;
using System.IO;
using System.Threading.Tasks;
using Fync.Client.Dispatcher;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.DispatchTasks
{
    internal class FolderSyncDispatchTask : FolderSyncDispatchTaskBase
    {
        protected readonly Folder _parentFolder;
        private readonly int _readonlyHashCode;

        public FolderSyncDispatchTask(DirectoryInfo localFolder, Folder parentFolder, Folder serverFolder, IDispatchFactory dispatchFactory, IDispatcher dispatcher, IHttpClient httpClient)
            : base(localFolder, serverFolder, dispatchFactory, dispatcher, httpClient)
        {
            _parentFolder = parentFolder;

            //Dont let the hashcode change if the serverFolder becomes populated
            _readonlyHashCode = (_localFolder.FullName + _serverFolder.SafeGet(x => x.Name, "null") + _parentFolder.Name).GetHashCode();
        }
        public override async Task Perform()
        {
            await SyncFolder();
            SyncSubFolders();
            await SyncFiles();
        }

        public override int GetHashCode()
        {
            return _readonlyHashCode;
        }

        public override bool Equals(object obj)
        {
            var that = obj as FolderSyncDispatchTask;
            return that != null
                   && that._localFolder.FullName.Equals(_localFolder.FullName, StringComparison.InvariantCultureIgnoreCase)
                   && that._serverFolder == _serverFolder
                   && that._parentFolder == _parentFolder;
        }

        private async Task SyncFolder()
        {
            if (_serverFolder == null)
            {
                //Doesnt exist on server
                _serverFolder = await _httpClient.PostAsync<Folder>("{0}/Folder".FormatWith(_parentFolder.Id), new { _localFolder.Name });
            }
            else if (!_localFolder.Exists && !_serverFolder.Deleted)
            {
                _localFolder.Create();
            }
            else if (_localFolder.Exists && _serverFolder.Deleted)
            {
                if (_localFolder.LastWriteTimeUtc < _serverFolder.ModifiedDate)
                {
                    _localFolder.Delete();
                    await _httpClient.DeleteAsync("Folder/{0}", _serverFolder.Id);
                }
            }
        }
    }
}