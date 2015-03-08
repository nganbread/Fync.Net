using System.IO;

namespace Fync.Client.Monitor
{
    internal class FolderMonitor : IFolderMonitor
    {
        private readonly FileSystemWatcher _folderWatcher;
        private readonly IClientConfiguration _clientConfiguration;

        public FolderMonitor(IClientConfiguration clientConfiguration)
        {
            _clientConfiguration = clientConfiguration;
            _folderWatcher = new FileSystemWatcher();
        }

        public void Monitor()
        {
            _folderWatcher.Path = _clientConfiguration.FyncDirectory.FullName;
            _folderWatcher.IncludeSubdirectories = true;
            _folderWatcher.EnableRaisingEvents = true;
            _folderWatcher.Filter = "*.*";
            _folderWatcher.NotifyFilter = NotifyFilters.DirectoryName;

            StopListening();
            StartListening();
        }

        private void StartListening()
        {
            _folderWatcher.Deleted += OnChanged;
            _folderWatcher.Renamed += OnChanged;
            _folderWatcher.Created += OnChanged;
        }

        private void OnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            QueueFileSystemChangeDispatchTask(fileSystemEventArgs);
        }

        private void QueueFileSystemChangeDispatchTask(FileSystemEventArgs fileSystemEventArgs)
        {
            switch (fileSystemEventArgs.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    {
                        var fileInfo = new DirectoryInfo(fileSystemEventArgs.FullPath);
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("Folder\tChange {0}", fileSystemEventArgs.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Created:
                    {
                        var fileInfo = new DirectoryInfo(fileSystemEventArgs.FullPath);
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("Folder\tCreated {0}", fileSystemEventArgs.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Renamed:
                    {
                        var fileInfo = new DirectoryInfo(fileSystemEventArgs.FullPath);
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("Folder\tRenamed {0}", fileSystemEventArgs.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Deleted:
                    Logger.Instance.Log("Folder\tDeleted {0}", fileSystemEventArgs.FullPath);

                    //_dispatcher.Enqueue(_dispathFactory.FolderSync(fileSystemEventArgs.FullPath));
                    break;
                    //trigger an update on that folder
            }
        }


        public void Dispose()
        {
            StopListening();
        }

        private void StopListening()
        {
            _folderWatcher.Deleted -= OnChanged;
            _folderWatcher.Renamed -= OnChanged;
            _folderWatcher.Created -= OnChanged;
            _folderWatcher.Changed -= OnChanged;
        }
    }
}