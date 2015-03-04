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
        protected readonly FolderWithChildren ParentFolder;
        private readonly int _readonlyHashCode;

        public FolderSyncDispatchTask(DirectoryInfo localFolder, FolderWithChildren parentFolder, FolderWithChildren serverFolder, IDispatchFactory dispatchFactory, IDispatcher dispatcher, IHttpClient httpClient)
            : base(localFolder, serverFolder, dispatchFactory, dispatcher, httpClient)
        {
            ParentFolder = parentFolder;

            //Dont let the hashcode change if the serverFolder becomes populated
            _readonlyHashCode = (_localFolder.FullName + ServerFolder.SafeGet(x => x.Name, "null") + ParentFolder.Name).GetHashCode();
        }

        public override async Task PerformAsync()
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
                   && that.ServerFolder == ServerFolder
                   && that.ParentFolder == ParentFolder;
        }

        private async Task SyncFolder()
        {
            if (ServerFolder == null)
            {
                //Doesnt exist on server
                ServerFolder = await _httpClient.PostAsync<FolderWithChildren>("{0}/Folder".FormatWith(ParentFolder.Id), new { _localFolder.Name });
            }
            else if (!_localFolder.Exists && !ServerFolder.Deleted)
            {
                _localFolder.Create();
            }
            else if (_localFolder.Exists && ServerFolder.Deleted)
            {
                if (_localFolder.LastWriteTimeUtc < ServerFolder.ModifiedDate)
                {
                    _localFolder.Delete();
                    await _httpClient.DeleteAsync("Folder/{0}", ServerFolder.Id);
                }
            }
        }

    }
}