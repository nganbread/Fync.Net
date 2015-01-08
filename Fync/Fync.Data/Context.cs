using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Fync.Common;
using Fync.Data.Models;

namespace Fync.Data
{
    internal class Context : DbContext, IContext
    {
        public Context()
            : base("FyncDatabase")
        {
            
        }

        public IDbSet<Folder> Folders { get; set; }

        public Folder GetTree(Folder root)
        {
            var script = "DECLARE @rootId hierarchyId " +
                         "SELECT @rootId = [HierarchyNode] " +
                         "FROM [dbo].[Folder] " +
                         "WHERE [Id] = '{0}' ".FormatWith(root.Id) + 

                         "SELECT * " +
                         "FROM [dbo].[Folder] " +
                         "WHERE [HierarchyNode].IsDescendantOf(@rootId) = 1 OR [Id] = '{0}'".FormatWith(root.Id);

            //var flat = ((DbSet<Folder>) Folders).SqlQuery(script).ToList(); //Change tracked
            var flat = Database.SqlQuery<Folder>(script).ToList();
            var children = flat.ToLookup(x => x.Parent_Id.GetValueOrDefault()); //Not change tracked

            var newRoot = flat.Single(x => x.Id == root.Id);
            FindAndAttachChildren(newRoot, children);

            return newRoot;
        }

        void IContext.SaveChanges()
        {
            SaveChanges();
        }

        public void FindAndAttachChildren(Folder root, ILookup<Guid, Folder> children)
        {
            if (children.Contains(root.Id))
            {
                root.SubFolders = children[root.Id].ToList();
                foreach (var subFolder in root.SubFolders)
                {
                    FindAndAttachChildren(subFolder, children);
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Folder>().HasMany(x => x.SubFolders).WithOptional(x => x.Parent);
            modelBuilder.Entity<Folder>().MapToStoredProcedures(x => x
                .Insert(y => y.HasName("Folder_Insert"))
                .Update(y => y.HasName("Folder_Update"))
                .Delete(y => y.HasName("Folder_Delete")));
        }
    }
}
