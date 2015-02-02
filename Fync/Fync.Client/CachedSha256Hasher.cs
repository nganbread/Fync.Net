using System;
using System.IO;
using System.Threading.Tasks;
using Fync.Client.DataBase;
using Fync.Common;

namespace Fync.Client
{
    internal class CachedSha256Hasher : Sha256Hasher
    {
        private readonly ILocalDatabase _localDatabase;

        public CachedSha256Hasher(ILocalDatabase localDatabase)
        {
            _localDatabase = localDatabase;
        }
        
        public override string Hash(string filePath)
        {
            var hash = base.Hash(filePath);
            _localDatabase.InsertHash(hash, filePath);
            return hash;
        }

        public override async Task<string> HashAsync(string filePath)
        {
            var hash = await base.HashAsync(filePath);
            await _localDatabase.InsertHashAsync(hash, filePath);
            return hash;
        }
    }
}