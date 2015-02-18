using System.Collections.Generic;
using Fync.Client.DispatchTasks;

namespace Fync.Client.Dispatcher
{
    internal interface IDispatcher
    {
        void Queue(IDispatchTask task);
        void Queue(IEnumerable<IDispatchTask> tasks);
        void Start();
    }
}