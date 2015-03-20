using System.IO;
using System.Threading.Tasks;

namespace Fync.Client.Monitor
{
    interface IMonitor<T>
    {
        Task Create(T target);
        Task Rename(T old, T @new);
        Task Update(T target);
        Task Delete(T target);
    }
}