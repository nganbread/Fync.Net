using System;

namespace Fync.Client
{
    public interface IFolderMonitor : IDisposable
    {
        void Monitor();
    }
}