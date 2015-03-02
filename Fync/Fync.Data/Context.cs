using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using Fync.Common;
using Fync.Data.Identity;
using Fync.Service.Models.Data;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fync.Data
{
    internal class Context : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>, IContext
    {
        public Context()
            : base("FyncDatabase")
        {
            
        }

        public IDbSet<FolderEntity> Folders { get; set; }

        public FolderEntity GetTree(int userId)
        {
            var script = "SELECT *" +
                         "FROM [dbo].[Folder]" +
                         "WHERE [OwnerId] = {0}".FormatWith(userId);

            var flat = ((DbSet<FolderEntity>)Folders).SqlQuery(script).ToList(); //Change tracked
            //var flat = Database.SqlQuery<FolderEntity>(script).ToList(); //Not change tracked

            var children = flat.ToLookup(x => x.ParentId.GetValueOrDefault());

            var root = flat.Single(x => !x.ParentId.HasValue);
            FindAndAttachChildren(root, children);

            return root;
        }

        public FolderEntity GetTree(int userId, Guid folderId)
        {
            var script = "DECLARE @rootId hierarchyId " +
                         "SELECT @rootId = [HierarchyNode] " +
                         "FROM [dbo].[Folder] " +
                         "WHERE [OwnerId] = {0} AND [Id] = '{1}'".FormatWith(userId, folderId) +

                         "SELECT * " +
                         "FROM [dbo].[Folder] " +
                         "WHERE [HierarchyNode].IsDescendantOf(@rootId) = 1 OR [Id] = '{0}'".FormatWith(folderId);

            var flat = ((DbSet<FolderEntity>)Folders).SqlQuery(script).ToList(); //Change tracked
            //var flat = Database.SqlQuery<FolderEntity>(script).ToList(); //Not change tracked
            var children = flat.ToLookup(x => x.ParentId.GetValueOrDefault());

            var root = flat.Single(x => x.Id == folderId);
            FindAndAttachChildren(root, children);

            return root;
        }


        /// <summary>
        /// TODO: protect from SQL Injection
        /// TODO: ?? Perform entirely in sql ??
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pathComponents"></param>
        /// <returns></returns>
        public FolderEntity GetFolderFromPath(int userId, string[] pathComponents)
        {
            var script =
                "SELECT DISTINCT ancestors.[Id], ancestors.[OwnerId], ancestors.[ParentId], ancestors.[Name], ancestors.[ModifiedDate], ancestors.[Deleted] " +
                "FROM [dbo].[Folder] as matching " +
                "RIGHT JOIN [dbo].[Folder] as ancestors " +
                "ON matching.[hierarchyNode].IsDescendantOf(ancestors.[HierarchyNode]) = 1 " +
                "WHERE " +
                    "matching.[OwnerId] = {0} AND ".FormatWith(userId) +
                    "matching.[Name] = '{0}' AND ".FormatWith(pathComponents.Last()) +
                    "matching.[HierarchyNode].GetLevel() = {0} ".FormatWith(pathComponents.Length - 1);

            //Perform entirely in SQL?
            var flat = ((DbSet<FolderEntity>)Folders).SqlQuery(script).ToList(); //Change tracked
            //var flat = Database.SqlQuery<FolderEntity>(script).ToList(); //Not change tracked

            var dictionary = flat.ToDictionary(x => x.Id);
            return flat
                .Where(x => x.Name.Equals(pathComponents.Last(), StringComparison.InvariantCultureIgnoreCase))
                .Select(x => 
                { 
                    FindAndAttachParent(x, dictionary);
                    return x;
                })
                .FirstOrDefault(x => IsMatchingPath(x, pathComponents, pathComponents.Length - 1));
        }

        private bool IsMatchingPath(FolderEntity folder, string[] path, int index)
        {
            if (folder == null) return true;
            return folder.Name == path[index] && IsMatchingPath(folder.Parent, path, index - 1);
        }

        private void FindAndAttachParent(FolderEntity folder, IDictionary<Guid, FolderEntity> dictionary)
        {
            if (!folder.ParentId.HasValue) return;
            folder.Parent = dictionary[folder.ParentId.Value];
            FindAndAttachParent(folder.Parent, dictionary);
        }

        void IContext.SaveChanges()
        {
            try
            {
                SaveChanges();

            }
            catch (Exception e)
            {
                var x = ChangeTracker.Entries<FolderEntity>().ToList();
                throw e;
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
            modelBuilder.Entity<FolderEntity>().HasMany(x => x.SubFolders).WithOptional(x => x.Parent);
            modelBuilder.Entity<FolderEntity>().HasRequired(x => x.Owner).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<FolderEntity>().MapToStoredProcedures(x => x
                .Insert(y => y.HasName("Folder_Insert"))
                .Update(y => y.HasName("Folder_Update"))
                .Delete(y => y.HasName("Folder_Delete")));
        }
    }
}
