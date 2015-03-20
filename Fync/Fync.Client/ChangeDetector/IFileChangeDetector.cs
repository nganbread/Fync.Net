using System.IO;

namespace Fync.Client.Monitor
{
    internal interface IFileChangeDetector
    {
        void Monitor(IMonitor<FileInfo> monitor);
    }
}