using System.IO;
using Fync.Client.DispatchTasks;
using Fync.Common.Models;

namespace Fync.Client.Dispatcher
{
    internal interface IDispatchFactory
    {
        IDispatchTask FileSync(FolderWithChildren parentFolder, FileInfo localFile, SymbolicFile serverFile);
        IDispatchTask FileSync(FolderWithChildren parentFolder, string filePath, SymbolicFile serverFile);
        IDispatchTask FolderSync(DirectoryInfo localFolder, FolderWithChildren parentFolder, FolderWithChildren serverFolder);
        IDispatchTask FolderSync(string folderPath, FolderWithChildren parentFolder, FolderWithChildren serverFolder);
        IDispatchTask RootFolderSync();
    }
}