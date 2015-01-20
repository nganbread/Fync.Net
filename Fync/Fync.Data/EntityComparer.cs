using System.Collections.Generic;
using Fync.Data.Models;

namespace Fync.Data
{
    public class EntityComparer : IEqualityComparer<DatabaseEntity>
    {
        private static readonly EntityComparer _instance = new EntityComparer();
        private EntityComparer(){}

        public static EntityComparer Instance
        {
            get { return _instance; }
        }

        public bool Equals(DatabaseEntity x, DatabaseEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(DatabaseEntity obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
