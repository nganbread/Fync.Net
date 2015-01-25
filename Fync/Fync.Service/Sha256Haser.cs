using System;
using System.IO;
using System.Security.Cryptography;

namespace Fync.Data
{
    internal class Sha256Haser : IHasher
    {
        public string Hash(Stream stream)
        {
            var hash = new SHA256Managed().ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}