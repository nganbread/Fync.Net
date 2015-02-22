using System;

namespace Fync.Client
{
    public interface IFileMonitor : IDisposable
    {
        void Monitor();
    }
}