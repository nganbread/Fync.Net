using System;
using System.Collections.Generic;
using System.IO;
using Fync.Client.DispatchTasks;
using Fync.Client.Extensions;
using Fync.Common.Models;

namespace Fync.Client.Dispatcher
{
    internal class DispatchFactory : IDispatchFactory
    {
        private readonly IClientConfiguration _clientConfiguration;
        private readonly Func<string, IDictionary<string, object>, FileSyncDispatchTask> _fileSyncDispatchTaskFactory;
        private readonly Func<string, IDictionary<string, object>, FolderSyncDispatchTask> _folderSyncDispatchTaskFactory;
        private readonly Func<RootFolderSyncDispatchTask> _rootFolderSyncDispatchTaskFactory;

        public DispatchFactory(IClientConfiguration clientConfiguration, 
            Func<string, IDictionary<string, object>, FileSyncDispatchTask> fileSyncDispatchTaskFactory, 
            Func<string, IDictionary<string, object>, FolderSyncDispatchTask> folderSyncDispatchTaskFactory, 
            Func<RootFolderSyncDispatchTask> rootFolderSyncDispatchTaskFactory)
        {
            _clientConfiguration = clientConfiguration;
            _fileSyncDispatchTaskFactory = fileSyncDispatchTaskFactory;
            _folderSyncDispatchTaskFactory = folderSyncDispatchTaskFactory;
            _rootFolderSyncDispatchTaskFactory = rootFolderSyncDispatchTaskFactory;
        }

        public IDispatchTask FileSync(FolderWithChildren parentFolderWithChildren, string filePath, SymbolicFile serverFile)
        {
            return FileSync(parentFolderWithChildren, CreateFileInfo(filePath), serverFile);
        }
        public IDispatchTask FileSync(FolderWithChildren parentFolderWithChildren, FileInfo localFile, SymbolicFile serverFile)
        {
            return _fileSyncDispatchTaskFactory(string.Empty, new Dictionary<string, object>
            {
                {"localFile", localFile},
                {"serverFile", serverFile},
                {"parentFolder", parentFolderWithChildren},

            });
        }

        private FileInfo CreateFileInfo(string filePath)
        {
            return _clientConfiguration.BaseDirectory.CreateFileInfo(filePath);
        }
        
        public IDispatchTask FolderSync(string workingDirectory, FolderWithChildren parentFolderWithChildren, FolderWithChildren serverFolderWithChildren)
        {
            return FolderSync(CreateDirectoryInfo(workingDirectory), parentFolderWithChildren, serverFolderWithChildren);
        }

        public IDispatchTask RootFolderSync()
        {
            return _rootFolderSyncDispatchTaskFactory();
        }

        public IDispatchTask FolderSync(DirectoryInfo localFolder, FolderWithChildren parentFolderWithChildren, FolderWithChildren serverFolderWithChildren)
        {
            return _folderSyncDispatchTaskFactory(string.Empty, new Dictionary<string, object>
            {
                {"localFolder", localFolder},
                {"parentFolder", parentFolderWithChildren},
                {"serverFolder", serverFolderWithChildren},
            });
        }

        private DirectoryInfo CreateDirectoryInfo(string workingDirectory)
        {
            return _clientConfiguration.BaseDirectory.CreateSubdirectoryInfo(workingDirectory);
        }
    }
}