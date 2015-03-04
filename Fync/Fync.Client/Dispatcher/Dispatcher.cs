using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fync.Client.DispatchTasks;

namespace Fync.Client.Dispatcher
{
    internal class Dispatcher : IDispatcher
    {
        private readonly ConcurrentQueue<IDispatchTask> _dispatchItems;

        public Dispatcher()
        {
            _dispatchItems = new ConcurrentQueue<IDispatchTask>();
        }
        public void Enqueue(IDispatchTask task)
        {
            _dispatchItems.Enqueue(task);
        }

        public void Enqueue(IEnumerable<IDispatchTask> tasks)
        {
            foreach (var task in tasks)
            {
                _dispatchItems.Enqueue(task);
            }
        }

        public void Start()
        {
            IDispatchTask task;
            while (_dispatchItems.TryDequeue(out task))
            {
                var enclosedTask = task;
                Task.Run(() => enclosedTask.PerformAsync()).Wait();
            }

            Thread.Sleep(5000);
            Start();
        }
    }
}