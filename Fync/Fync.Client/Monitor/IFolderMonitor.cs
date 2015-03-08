using System;

namespace Fync.Client.Monitor
{
    public interface IFolderMonitor : IDisposable
    {
        void Monitor();
    }
}