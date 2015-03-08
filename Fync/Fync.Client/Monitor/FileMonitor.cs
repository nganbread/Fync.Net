using System.IO;

namespace Fync.Client.Monitor
{
    internal class FileMonitor : IFileMonitor
    {
        private readonly FileSystemWatcher _fileWatcher;
        private readonly IClientConfiguration _clientConfiguration;

        public FileMonitor(IClientConfiguration clientConfiguration)
        {
            _clientConfiguration = clientConfiguration;
            _fileWatcher = new FileSystemWatcher();
        }

        public void Monitor()
        {
            _fileWatcher.Path = _clientConfiguration.FyncDirectory.FullName;
            _fileWatcher.IncludeSubdirectories = true;
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.Filter = "*.*";
            _fileWatcher.NotifyFilter =
                NotifyFilters.FileName |
                NotifyFilters.LastWrite |
                NotifyFilters.Size;

            StopListening();
            StartListening();
        }

        private void StartListening()
        {
            _fileWatcher.Deleted += OnChanged;
            _fileWatcher.Renamed += OnChanged;
            _fileWatcher.Changed += OnChanged;
        }

        private void OnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            QueueFileSystemChangeDispatchTask(fileSystemEventArgs);
        }

        private static void QueueFileSystemChangeDispatchTask(FileSystemEventArgs fileSystemEventArgs)
        {
            switch (fileSystemEventArgs.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    {
                        var fileInfo = new FileInfo(fileSystemEventArgs.FullPath);
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("File\tChange {0}", fileSystemEventArgs.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Created:
                    {
                        var fileInfo = new FileInfo(fileSystemEventArgs.FullPath);
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("File\tCreated {0}", fileSystemEventArgs.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Renamed: 
                    { 
                        var fileInfo = new FileInfo(fileSystemEventArgs.FullPath);
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("File\tRenamed {0}", fileSystemEventArgs.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Deleted:
                    Logger.Instance.Log("File\tDeleted {0}", fileSystemEventArgs.FullPath);
                    break;
            }
        }

        public void Dispose()
        {
            StopListening();
        }

        private void StopListening()
        {
            _fileWatcher.Deleted -= OnChanged;
            _fileWatcher.Renamed -= OnChanged;
            //_fileWatcher.Created -= OnChanged;
            _fileWatcher.Changed -= OnChanged;
        }
    }
}