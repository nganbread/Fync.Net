using System.IO;
using Fync.Client.DispatchTasks;
using Fync.Common.Models;

namespace Fync.Client.Dispatcher
{
    internal interface IDispatchFactory
    {
        IDispatchTask FileSync(Folder parentFolder, FileInfo localFile, SymbolicFile serverFile);
        IDispatchTask FileSync(Folder parentFolder, string filePath, SymbolicFile serverFile);
        IDispatchTask FolderSync(DirectoryInfo localFolder, Folder parentFolder, Folder serverFolder);
        IDispatchTask FolderSync(string folderPath, Folder parentFolder, Folder serverFolder);
        IDispatchTask RootFolderSync();
    }
}