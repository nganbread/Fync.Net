using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.DispatchTasks;

namespace Fync.Client.Dispatcher
{
    internal class Dispatcher : IDispatcher
    {
        private readonly HashSet<IDispatchTask> _dispatchItems;
        private bool _processing;

        public Dispatcher()
        {
            _dispatchItems = new HashSet<IDispatchTask>();
        }
        public void Queue(IDispatchTask task)
        {
            _dispatchItems.Add(task);

            Start();
        }

        public void Queue(IEnumerable<IDispatchTask> tasks)
        {
            foreach (var task in tasks)
            {
                _dispatchItems.Add(task);
            }

            Start();
        }

        public void Start()
        {
            if (_processing) return;

            _processing = true;
            while (_dispatchItems.Any())
            {
                var next = _dispatchItems.First();
                _dispatchItems.Remove(next);
                Task.Run(() => next.PerformAsync()).Wait();
            }
            _processing = false;
        }
    }
}