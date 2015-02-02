using System;
using System.Threading;
using Fync.Client.Dispatcher;

namespace Fync.Client
{
    internal class Application
    {
        private readonly IDispatcher _dispatcher;
        private readonly IDispatchFactory _dispatchFactory;

        public Application(IDispatcher dispatcher, IDispatchFactory dispatchFactory)
        {
            _dispatcher = dispatcher;
            _dispatchFactory = dispatchFactory;
        }

        public void Start()
        {
            try
            {
                _dispatcher.Add(_dispatchFactory.RootFolderSync());
            }
            catch (Exception e)
            {
                
            }
            
        }
    }
}