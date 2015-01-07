using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Fync.Data.Models;

namespace Fync.Data
{
    internal class Context : DbContext
    {
        public Context()
            : base("FyncDatabase")
        {
            
        }

        public IDbSet<Folder> Folders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
