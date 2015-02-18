using System;
using System.Threading;
using Fync.Client.Dispatcher;

namespace Fync.Client
{
    internal class SyncEngine : ISyncEngine
    {
        private readonly IDispatcher _dispatcher;
        private readonly IDispatchFactory _dispatchFactory;
        private readonly IFolderMonitor _folderMonitor;
        private readonly IFileMonitor _fileMonitor;

        public SyncEngine(IDispatcher dispatcher, IDispatchFactory dispatchFactory, IFolderMonitor folderMonitor, IFileMonitor fileMonitor)
        {
            _dispatcher = dispatcher;
            _dispatchFactory = dispatchFactory;
            _folderMonitor = folderMonitor;
            _fileMonitor = fileMonitor;
        }

        public void Start()
        {
            Logger.Instance.Log("Start");

            _dispatcher.Queue(_dispatchFactory.RootFolderSync());
            //_fileMonitor.Monitor();
            //_folderMonitor.Monitor();
        }
    }
}