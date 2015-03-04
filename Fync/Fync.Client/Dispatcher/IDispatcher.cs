using System.Collections.Generic;
using Fync.Client.DispatchTasks;

namespace Fync.Client.Dispatcher
{
    internal interface IDispatcher
    {
        void Enqueue(IDispatchTask task);
        void Enqueue(IEnumerable<IDispatchTask> tasks);
        void Start();
    }
}