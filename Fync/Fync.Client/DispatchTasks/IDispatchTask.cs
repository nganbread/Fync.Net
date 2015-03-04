using System.Threading.Tasks;

namespace Fync.Client.DispatchTasks
{
    internal interface IDispatchTask
    {
        Task PerformAsync();
    }
}