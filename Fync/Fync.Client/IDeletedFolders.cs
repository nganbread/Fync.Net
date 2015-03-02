using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fync.Client
{
    internal interface IDeletedFolders
    {
        void Add(string path);
    }

    internal class DeletedFolders : IDeletedFolders
    {
        private readonly IDictionary<string, DateTime> _deletedFolders;

        public DeletedFolders()
        {
            _deletedFolders = new Dictionary<string, DateTime>();
        }
        public void Add(string path)
        {
            _deletedFolders.Add(path, DateTime.UtcNow);
        }
    }
}
