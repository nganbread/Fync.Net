using System;
using System.Collections.Generic;
using System.IO;
using Fync.Service.Models;
using File = Fync.Service.Models.File;

namespace Fync.Service
{
    public interface ISymbolicFileService
    {
        IList<SymbolicFile> GetFilesInFolder(Guid folderId);
        SymbolicFile GetFileOrDefault(Guid folderId, string fileName);
        void AddFileToFolder(Guid folderId, NewSymbolicFile symbolicFile);
        void CreateFile(Stream stream, Guid folderId, string fileName);
        void DeleteSymbolicFilesFromFolder(Guid folderId);
        void DeleteSymbolicFileFromFolder(Guid folderId, string fileName);
        File GetFile(Guid folderId, string fileName);
    }
}