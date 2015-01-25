using System.IO;

namespace Fync.Data
{
    public interface IHasher
    {
        string Hash(Stream stream);
    }
}