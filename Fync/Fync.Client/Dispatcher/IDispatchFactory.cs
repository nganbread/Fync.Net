using System.IO;
using Fync.Client.DispatchTasks;
using Fync.Common.Models;

namespace Fync.Client.Dispatcher
{
    internal interface IDispatchFactory
    {
        IDispatchTask FileSync(FolderWithChildren parentFolderWithChildren, FileInfo localFile, SymbolicFile serverFile);
        IDispatchTask FileSync(FolderWithChildren parentFolderWithChildren, string filePath, SymbolicFile serverFile);
        IDispatchTask FolderSync(DirectoryInfo localFolder, FolderWithChildren parentFolderWithChildren, FolderWithChildren serverFolderWithChildren);
        IDispatchTask FolderSync(string folderPath, FolderWithChildren parentFolderWithChildren, FolderWithChildren serverFolderWithChildren);
        IDispatchTask RootFolderSync();
    }
}