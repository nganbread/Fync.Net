using System;

namespace Fync.Client
{
    internal interface IFolderMonitor : IDisposable
    {
        void Monitor();
    }
}