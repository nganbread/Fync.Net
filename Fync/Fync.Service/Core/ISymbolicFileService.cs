using System;
using System.Collections.Generic;
using System.IO;
using Fync.Common.Models;
using File = Fync.Service.Models.File;

namespace Fync.Service
{
    public interface ISymbolicFileService
    {
        IList<SymbolicFile> GetFilesInFolder(Guid folderId);
        SymbolicFile GetFileOrDefault(Guid folderId, string fileName);
        SymbolicFile AddSymbolicFileToFolder(Guid folderId, NewSymbolicFile symbolicFile, DateTime dateCreated);
        SymbolicFile CreateSymbolicFile(Stream stream, Guid folderId, string fileName, DateTime dateCreated);
        void DeleteSymbolicFilesFromFolder(Guid folderId, DateTime dateDeleted);
        SymbolicFile DeleteSymbolicFileFromFolder(Guid folderId, string fileName, DateTime dateDeleted);
        File GetFile(Guid folderId, string fileName);
    }
}