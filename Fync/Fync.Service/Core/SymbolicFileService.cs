using System;
using System.Collections.Generic;
using System.IO;
using Fync.Common;
using Fync.Common.Models;
using Fync.Data;
using Fync.Data.Entities.Table;
using Microsoft.WindowsAzure.Storage;
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

        public SymbolicFile CreateSymbolicFile(Stream stream, Guid folderId, string fileName, DateTime dateCreated)
        {
            var hash = _hasher.Hash(stream);
            if (!_fileTableAccess.FileExists(hash))
            {
                //we dont have the file already
                var blob = _fileBlobAccess.UploadFile(stream);
                _fileTableAccess.CreateFile(hash, blob.Name);
            }

            return AddSymbolicFileToFolder(folderId, hash, fileName, dateCreated);
        }

        public IList<SymbolicFile> GetFilesInFolder(Guid folderId)
        {
            return _symbolicFileTableAccess.GetSymbolicFilesInFolder(folderId).MapToList(_toFile);
        }

        public SymbolicFile GetFileOrDefault(Guid folderId, string fileName)
        {
            return _symbolicFileTableAccess.GetSymbolicFileOrDefault(folderId, fileName).Map(_toFile);
        }

        public SymbolicFile AddSymbolicFileToFolder(Guid folderId, NewSymbolicFile symbolicFile, DateTime dateCreated)
        {
            if (!_fileTableAccess.FileExists(symbolicFile.Hash)) throw new Exception();

            return AddSymbolicFileToFolder(folderId, symbolicFile.Hash, symbolicFile.Name, dateCreated);
        }

        private SymbolicFile AddSymbolicFileToFolder(Guid folderId, string hash, string fileName, DateTime dateCreatedUtc)
        {
            DeleteSymbolicFileFromFolder(folderId, fileName, dateCreatedUtc);
            return _symbolicFileTableAccess.InsertSymbolicFileToFolder(folderId, hash, fileName, dateCreatedUtc).Map(_toFile);
        }

        public void DeleteSymbolicFilesFromFolder(Guid folderId, DateTime dateDeleted)
        {
            var symbolicFiles = _symbolicFileTableAccess.GetSymbolicFilesInFolder(folderId);
            _deletedSymbolicFileTableAccess.AddDeletedSymbolicFilesInFolderToFolder(symbolicFiles, dateDeleted);
            _symbolicFileTableAccess.DeleteSymbolicFilesFromFolder(symbolicFiles, dateDeleted);
        }

        public SymbolicFile DeleteSymbolicFileFromFolder(Guid folderId, string fileName, DateTime dateDeleted)
        {
            var symbolicFile = _symbolicFileTableAccess.GetSymbolicFileOrDefault(folderId, fileName);
            if (symbolicFile != null)
            {
                _deletedSymbolicFileTableAccess.AddDeletedSymbolicFileToFolder(symbolicFile, dateDeleted);
                return _symbolicFileTableAccess.DeleteSymbolicFileFromFolder(symbolicFile, dateDeleted).Map(_toFile);
            }
            else
            {
                return null;
            }
        }
    }
}