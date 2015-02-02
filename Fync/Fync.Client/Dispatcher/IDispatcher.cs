using System.Collections.Generic;
using Fync.Client.DispatchTasks;

namespace Fync.Client.Dispatcher
{
    internal interface IDispatcher
    {
        //use a set. dont allow duplicates
        void Add(IDispatchTask task);
        void Add(IEnumerable<IDispatchTask> tasks);
        void Start();
    }
}