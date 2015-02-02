using System.Threading.Tasks;

namespace Fync.Client.DispatchTasks
{
    internal abstract class DispatchTaskBase : IDispatchTask
    {
        public Task PerformAsync()
        {
            return Task.Run(() => Perform());
        }

        public abstract int Priority { get; }
        public abstract Task Perform();
    }
}