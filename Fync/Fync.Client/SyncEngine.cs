using System;
using System.Threading;
using Fync.Client.Dispatcher;

namespace Fync.Client
{
    internal class SyncEngine : ISyncEngine
    {
        private readonly IDispatcher _dispatcher;
        private readonly IDispatchFactory _dispatchFactory;

        public SyncEngine(IDispatcher dispatcher, IDispatchFactory dispatchFactory)
        {
            _dispatcher = dispatcher;
            _dispatchFactory = dispatchFactory;
        }

        public void Start()
        {
            Logger.Instance.Log("Start");

            _dispatcher.Enqueue(_dispatchFactory.RootFolderSync());
            _dispatcher.Start();
        }
    }
}