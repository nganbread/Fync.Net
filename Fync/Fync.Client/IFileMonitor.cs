using System;

namespace Fync.Client
{
    internal interface IFileMonitor : IDisposable
    {
        void Monitor();
    }
}