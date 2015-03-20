using System.Collections.Generic;
using System.IO;
using Fync.Common;
using Fync.Common.Common;

namespace Fync.Client.Monitor
{
    internal class FolderChangeDetector : IFolderChangeDetector
    {
        private readonly FileSystemWatcher _folderWatcher;
        private readonly IList<IMonitor<DirectoryInfo>> _monitors;

        public FolderChangeDetector(IClientConfiguration clientConfiguration)
        {
            _monitors = new List<IMonitor<DirectoryInfo>>();
            _folderWatcher = new FileSystemWatcher
            {
                Path = clientConfiguration.FyncDirectory.FullName,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
                Filter = "*.*",
                NotifyFilter = NotifyFilters.DirectoryName
            };

            StopListening();
            StartListening();
        }

        public void Monitor(IMonitor<DirectoryInfo> monitor)
        {
            _monitors.Add(monitor);
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
            var fileInfo = new DirectoryInfo(fileSystemEventArgs.FullPath);
            switch (fileSystemEventArgs.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    {
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("Folder\tChange {0}", fileSystemEventArgs.FullPath);
                        _monitors.ForEach(x => x.Update(fileInfo));
                        break;
                    }
                case WatcherChangeTypes.Created:
                    {
                        if (!fileInfo.Exists) return;
                        Logger.Instance.Log("Folder\tCreated {0}", fileSystemEventArgs.FullPath);
                        _monitors.ForEach(x => x.Create(fileInfo));
                        break;
                    }
                case WatcherChangeTypes.Renamed:
                    {
                        if (!fileInfo.Exists) return;
                        if (!(fileSystemEventArgs is RenamedEventArgs)) return;
                        Logger.Instance.Log("Folder\tRenamed {0}", fileSystemEventArgs.FullPath);
                        var oldFileInfo = new DirectoryInfo(((RenamedEventArgs) fileSystemEventArgs).OldFullPath);
                        _monitors.ForEach(x => x.Rename(oldFileInfo, fileInfo));
                        break;
                    }
                case WatcherChangeTypes.Deleted:
                    Logger.Instance.Log("Folder\tDeleted {0}", fileSystemEventArgs.FullPath);
                    _monitors.ForEach(x => x.Delete(fileInfo));
                    break;
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