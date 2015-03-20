using System.Collections.Generic;
using System.IO;
using Fync.Common;
using Fync.Common.Common;

namespace Fync.Client.Monitor
{
    internal class FileChangeDetector : IFileChangeDetector
    {
        private readonly FileSystemWatcher _fileWatcher;
        private readonly IList<IMonitor<FileInfo>> _monitors;

        public FileChangeDetector(IClientConfiguration clientConfiguration)
        {
            _monitors = new List<IMonitor<FileInfo>>();
            _fileWatcher = new FileSystemWatcher
            {
                Path = clientConfiguration.FyncDirectory.FullName,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
                Filter = "*.*",
                NotifyFilter = NotifyFilters.FileName |
                               NotifyFilters.LastWrite |
                               NotifyFilters.Size
            };

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
            var fileInfo = new FileInfo(fileSystemEventArgs.FullPath);
            switch (fileSystemEventArgs.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    {
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("File\tChanged {0}", fileSystemEventArgs.FullPath);
                        _monitors.ForEach(x => x.Update(fileInfo));
                        break;
                    }
                case WatcherChangeTypes.Created:
                    {
                        if (!fileInfo.Exists) return;
                        _monitors.ForEach(x => x.Create(fileInfo));
                        Logger.Instance.Log("File\tCreated {0}", fileSystemEventArgs.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Renamed: 
                    { 
                        if (!fileInfo.Exists) return;
                        _monitors.ForEach(x => x.Update(fileInfo));
                        Logger.Instance.Log("File\tRenamed {0}", fileSystemEventArgs.FullPath);
                        break;
                    }
                case WatcherChangeTypes.Deleted:
                {
                    _monitors.ForEach(x => x.Delete(fileInfo));
                    Logger.Instance.Log("File\tDeleted {0}", fileSystemEventArgs.FullPath);
                    break;
                }
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
            _fileWatcher.Changed -= OnChanged;
        }

        public void Monitor(IMonitor<FileInfo> monitor)
        {
            _monitors.Add(monitor);
        }
    }
}