using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Fync.Common;
using Fync.Service.Models.Data;

namespace Fync.Data
{
    internal class Context : DbContext, IContext
    {
        public Context()
            : base("FyncDatabase")
        {
            
        }

        public IDbSet<FolderEntity> Folders { get; set; }

        public FolderEntity GetTree(FolderEntity root)
        {
            var script = "DECLARE @rootId hierarchyId " +
                         "SELECT @rootId = [HierarchyNode] " +
                         "FROM [dbo].[Folder] " +
                         "WHERE [Id] = '{0}' ".FormatWith(root.Id) + 

                         "SELECT * " +
                         "FROM [dbo].[Folder] " +
                         "WHERE [HierarchyNode].IsDescendantOf(@rootId) = 1 OR [Id] = '{0}'".FormatWith(root.Id);

            var flat = ((DbSet<FolderEntity>)Folders).SqlQuery(script).ToList(); //Change tracked
            //var flat = Database.SqlQuery<FolderEntity>(script).ToList(); //Not change tracked
            var children = flat.ToLookup(x => x.ParentId.GetValueOrDefault());

            var newRoot = flat.Single(x => x.Id == root.Id);
            FindAndAttachChildren(newRoot, children);

            return newRoot;
        }

        void IContext.SaveChanges()
        {
            try
            {
                SaveChanges();

            }
            catch (Exception e)
            {
                
            }
        }

        public void FindAndAttachChildren(FolderEntity root, ILookup<Guid, FolderEntity> children)
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

            modelBuilder.Entity<FolderEntity>().ToTable("Folder");
            modelBuilder.Entity<FolderEntity>().HasMany(x => x.SubFolders).WithRequired(x => x.Parent);
            modelBuilder.Entity<FolderEntity>().MapToStoredProcedures(x => x
                .Insert(y => y.HasName("Folder_Insert"))
                .Update(y => y.HasName("Folder_Update"))
                .Delete(y => y.HasName("Folder_Delete")));
        }
    }
}
