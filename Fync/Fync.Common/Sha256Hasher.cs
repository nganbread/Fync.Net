using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Fync.Common
{
    //use this instead? https://code.google.com/p/xxhash/
    public class Sha256Hasher : IHasher
    {
        public virtual string Hash(Stream stream)
        {
            var hash = new SHA256Managed().ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public virtual string Hash(string filePath)
        {
            using (var stream = new BufferedStream(File.OpenRead(filePath)))
            {
                return Hash(stream);
            }
        }

        public virtual string Hash(FileInfo filePath)
        {
            return Hash(filePath.FullName);
        }
    }
}