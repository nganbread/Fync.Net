using System;
using System.Collections.Generic;
using System.IO;
using Fync.Common;
using Fync.Data;
using Fync.Data.Entities.Table;
using Fync.Service.Models;
using File = Fync.Service.Models.File;

namespace Fync.Service
{
    internal class SymbolicFileService : ISymbolicFileService
    {
        private readonly IFileTableAccess _fileTableAccess;
        private readonly IHasher _hasher;
        private readonly ISymbolicFileTableAccess _symbolicFileTableAccess;
        private readonly IFileBlobAccess _fileBlobAccess;
        private readonly IDeletedSymbolicFileTableAccess _deletedSymbolicFileTableAccess;
        private readonly Func<SymbolicFileTableEntity, SymbolicFile> _toFile;

        public SymbolicFileService(IFileTableAccess fileTableAccess, IHasher hasher, ISymbolicFileTableAccess symbolicFileTableAccess, IFileBlobAccess fileBlobAccess, IDeletedSymbolicFileTableAccess deletedSymbolicFileTableAccess, Func<SymbolicFileTableEntity, SymbolicFile> toFile)
        {
            _fileTableAccess = fileTableAccess;
            _hasher = hasher;
            _symbolicFileTableAccess = symbolicFileTableAccess;
            _fileBlobAccess = fileBlobAccess;
            _deletedSymbolicFileTableAccess = deletedSymbolicFileTableAccess;
            _toFile = toFile;
        }

        public File GetFile(Guid folderId, string fileName)
        {
            var symbolicFile = GetFileOrDefault(folderId, fileName);
            if (symbolicFile == null) throw new Exception();

            var file = _fileTableAccess.GetFileOrDefault(symbolicFile.Hash);
            if (file == null) throw new Exception();

            var blob = _fileBlobAccess.GetBlob(file.BlobName);
            if (blob == null) throw new Exception();

            return new File
            {
                Stream = blob.OpenRead(),
                FileName = symbolicFile.Name,
                LengthInBytes = blob.Properties.Length,
                ContentType = blob.Properties.ContentType
            };
        }

        public void CreateFile(Stream stream, Guid folderId, string fileName)
        {
            var hash = _hasher.Hash(stream);
            if (!_fileTableAccess.FileExists(hash))
            {
                //we dont have the file already
                var blob = _fileBlobAccess.UploadFile(stream);
                _fileTableAccess.CreateFile(hash, blob.Name);
            }

            AddFileToFolder(folderId, hash, fileName);
        }

        public IList<SymbolicFile> GetFilesInFolder(Guid folderId)
        {
            return _symbolicFileTableAccess.GetSymbolicFilesInFolder(folderId).MapToList(_toFile);
        }

        public SymbolicFile GetFileOrDefault(Guid folderId, string fileName)
        {
            return _symbolicFileTableAccess.GetSymbolicFileOrDefault(folderId, fileName).Map(_toFile);
        }

        public void AddFileToFolder(Guid folderId, NewSymbolicFile symbolicFile)
        {
            if (!_fileTableAccess.FileExists(symbolicFile.Hash)) throw new Exception();

            AddFileToFolder(folderId, symbolicFile.Hash, symbolicFile.Name);
        }

        private void AddFileToFolder(Guid folderId, string hash, string fileName)
        {
            DeleteSymbolicFileFromFolder(folderId, fileName);
            _symbolicFileTableAccess.AddSymbolicFileToFolder(folderId, hash, fileName, DateTime.UtcNow);
        }

        public void DeleteSymbolicFilesFromFolder(Guid folderId)
        {
            var symbolicFiles = _symbolicFileTableAccess.GetSymbolicFilesInFolder(folderId);
            _deletedSymbolicFileTableAccess.AddDeletedSymbolicFilesInFolderToFolder(symbolicFiles, DateTime.UtcNow);
            _symbolicFileTableAccess.DeleteSymbolicFilesFromFolder(symbolicFiles);
        }

        public void DeleteSymbolicFileFromFolder(Guid folderId, string fileName)
        {
            var symbolicFile = _symbolicFileTableAccess.GetSymbolicFileOrDefault(folderId, fileName);
            if (symbolicFile != null)
            {
                _deletedSymbolicFileTableAccess.AddDeletedSymbolicFileToFolder(symbolicFile, DateTime.UtcNow);
                _symbolicFileTableAccess.DeleteSymbolicFileFromFolder(symbolicFile);
            }
        }
    }
}