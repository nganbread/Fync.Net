using System.IO;
using System.Threading.Tasks;

namespace Fync.Common
{
    public interface IHasher
    {
        string Hash(Stream stream);
        string Hash(string filePath);
        Task<string> HashAsync(string filePath);
    }
}