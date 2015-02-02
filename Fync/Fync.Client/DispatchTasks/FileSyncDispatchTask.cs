using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.DataBase;
using Fync.Client.HelperServices;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.DispatchTasks
{
    internal class FileSyncDispatchTask : DispatchTaskBase
    {
        private readonly Folder _parentFolder;
        private readonly FileInfo _localFile;
        private readonly SymbolicFile _serverFile;
        private readonly IFileHelper _fileHelper;
        private readonly IHttpClient _httpClient;
        private readonly IHasher _hasher;
        private readonly ILocalDatabase _localDataBase;

        public FileSyncDispatchTask(Folder parentFolder, FileInfo localFile, SymbolicFile serverFile, IFileHelper fileHelper, IHttpClient httpClient, IHasher hasher, ILocalDatabase localDataBase)
        {
            _parentFolder = parentFolder;
            _localFile = localFile;
            _serverFile = serverFile;
            _fileHelper = fileHelper;
            _httpClient = httpClient;
            _hasher = hasher;
            _localDataBase = localDataBase;
        }

        public override int GetHashCode()
        {
            return (_localFile.FullName + _serverFile.SafeGet(x => x.Name, "null") + _parentFolder.Name).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var that = obj as FileSyncDispatchTask;
            return that != null
                   && that._localFile.FullName.Equals(_localFile.FullName, StringComparison.InvariantCultureIgnoreCase)
                   && that._serverFile == _serverFile
                   && that._parentFolder == _parentFolder;
        }

        public override int Priority
        {
            get { return 3; }
        }

        public override async Task Perform()
        {
            if (_serverFile == null)
            {
                await UploadNewFile();
            }
            else if (!_localFile.Exists)
            {
                await DownloadFile();    
            }
            else if(_localFile.LastWriteTimeUtc > _serverFile.DateCreatedUtc)
            {
                await UploadExistingFile();
            }
            else
            {
                if (_serverFile.Hash == await _hasher.HashAsync(_localFile.FullName))
                {
                    //hashes are the same, file hasnt changed
                    return;
                }

                await DownloadFile();
            }
        }

        private async Task DownloadFile()
        {
            //check local hashes first
            var filePaths = await _localDataBase.FilePathsOfCachedHashAsync(_serverFile.Hash);
            var existingFile = filePaths.FirstOrDefault(x => x.Exists);
            if (existingFile != null)
            {
                var freshHash = await _hasher.HashAsync(existingFile.FullName);
                if (freshHash == _serverFile.Hash)
                {
                    //file exists on disk already
                    existingFile.CopyTo(_localFile.FullName);
                    return;
                }
            }

            //Download the file
            var fileStream = await _httpClient.GetStreamAsync("{0}/Data?fileName={1}", _parentFolder.Id, _serverFile.Name);
            await _fileHelper.SaveToDiskAsync(fileStream, _localFile);
            await _hasher.HashAsync(_localFile.FullName); //TODO:hash it while its in memory instead of from disk
        }

        /// <summary>
        /// There is no existing server version of the file
        /// </summary>
        /// <returns></returns>
        private async Task UploadNewFile()
        {
            var hash = await _hasher.HashAsync(_localFile.FullName);

            await Upload(hash);
        }

        private async Task Upload(string hash)
        {
            var fileAlreadyExistsOnServer = await _httpClient.GetAsync<bool>("File?hash={0}", hash);

            if (fileAlreadyExistsOnServer)
            {
                var newSymbolicFile = new NewSymbolicFile
                {
                    Hash = hash,
                    Name = _localFile.Name,
                };
                await _httpClient.PutAsync("{0}/SymbolicFile".FormatWith(_parentFolder.Id), newSymbolicFile);
            }
            else
            {
                await _httpClient.PostStreamAsync(_localFile.OpenRead(), new NewFile { Name = _localFile.Name }, "{0}/SymbolicFile", _parentFolder.Id);
            }
        }

        /// <summary>
        /// A version of this file already exists on the server
        /// </summary>
        /// <returns></returns>
        private async Task UploadExistingFile()
        {
            var hash = await _hasher.HashAsync(_localFile.FullName);

            if (hash == _serverFile.Hash)
            {
                //file hasnt changed. no point updating it - should we update the last write time?
                return;
            }

            await Upload(hash);
        }
    }
}