using System.IO;
using Fync.Common;

namespace Fync.Client.Hash
{
    internal class CachedSha256Hasher : Sha256Hasher
    {
        private readonly IHashCache _hashCache;

        public CachedSha256Hasher(IHashCache hashCache)
        {
            _hashCache = hashCache;
        }
        
        public override string Hash(string filePath)
        {
            var hash = base.Hash(filePath);
            _hashCache.InsertHash(hash, filePath);
            return hash;
        }
        
        public override string Hash(FileInfo filePath)
        {
            return Hash(filePath.FullName);
        }
    }
}