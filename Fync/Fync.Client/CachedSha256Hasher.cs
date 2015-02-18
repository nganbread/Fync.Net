using System;
using System.IO;
using System.Threading.Tasks;
using Fync.Client.DataBase;
using Fync.Common;

namespace Fync.Client
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

        public override async Task<string> HashAsync(string filePath)
        {
            var hash = await base.HashAsync(filePath);
            await _hashCache.InsertHashAsync(hash, filePath);
            return hash;
        }
    }
}