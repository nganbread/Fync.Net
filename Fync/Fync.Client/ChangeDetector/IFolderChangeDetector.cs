using System.IO;

namespace Fync.Client.Monitor
{
    internal interface IFolderChangeDetector
    {
        void Monitor(IMonitor<DirectoryInfo> monitor);
    }
}