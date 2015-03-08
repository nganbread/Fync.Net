using System.Threading.Tasks;

namespace Fync.Client
{
    public interface ISyncEngine
    {
        Task Start();
    }
}