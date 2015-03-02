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
        protected readonly FolderWithChildren ParentFolderWithChildren;
        private readonly int _readonlyHashCode;

        public FolderSyncDispatchTask(DirectoryInfo localFolder, FolderWithChildren parentFolderWithChildren, FolderWithChildren serverFolderWithChildren, IDispatchFactory dispatchFactory, IDispatcher dispatcher, IHttpClient httpClient)
            : base(localFolder, serverFolderWithChildren, dispatchFactory, dispatcher, httpClient)
        {
            ParentFolderWithChildren = parentFolderWithChildren;

            //Dont let the hashcode change if the serverFolder becomes populated
            _readonlyHashCode = (_localFolder.FullName + ServerFolderWithChildren.SafeGet(x => x.Name, "null") + ParentFolderWithChildren.Name).GetHashCode();
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
                   && that.ServerFolderWithChildren == ServerFolderWithChildren
                   && that.ParentFolderWithChildren == ParentFolderWithChildren;
        }

        private async Task SyncFolder()
        {
            if (ServerFolderWithChildren == null)
            {
                //Doesnt exist on server
                ServerFolderWithChildren = await _httpClient.PostAsync<FolderWithChildren>("{0}/Folder".FormatWith(ParentFolderWithChildren.Id), new { _localFolder.Name });
            }
            else if (!_localFolder.Exists && !ServerFolderWithChildren.Deleted)
            {
                _localFolder.Create();
            }
            else if (_localFolder.Exists && ServerFolderWithChildren.Deleted)
            {
                if (_localFolder.LastWriteTimeUtc < ServerFolderWithChildren.ModifiedDate)
                {
                    _localFolder.Delete();
                    await _httpClient.DeleteAsync("Folder/{0}", ServerFolderWithChildren.Id);
                }
            }
        }
    }
}