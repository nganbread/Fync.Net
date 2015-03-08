using System;

namespace Fync.Client.Monitor
{
    public interface IFileMonitor : IDisposable
    {
        void Monitor();
    }
}