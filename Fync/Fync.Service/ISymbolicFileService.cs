﻿using System;
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
        void AddSymbolicFileToFolder(Guid folderId, NewSymbolicFile symbolicFile, DateTime dateCreated);
        void CreateSymbolicFile(Stream stream, Guid folderId, string fileName, DateTime dateCreated);
        void DeleteSymbolicFilesFromFolder(Guid folderId, DateTime dateDeleted);
        void DeleteSymbolicFileFromFolder(Guid folderId, string fileName, DateTime dateDeleted);
        File GetFile(Guid folderId, string fileName);
    }
}