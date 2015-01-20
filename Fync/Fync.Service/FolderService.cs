using System;
using System.Linq;
using Fync.Common;
using Fync.Data;
using Fync.Service.Models;
using Fync.Service.Models.Data;

namespace Fync.Service
{

    internal class FolderService : IFolderService
    {
        private readonly IContext _context;
        private readonly Func<FolderEntity, Folder> _toFolder;
        private readonly Func<Folder, FolderEntity> _toFolderEntity;

        public FolderService(IContext context, Func<FolderEntity, Folder> toFolder, Func<Folder, FolderEntity> toFolderEntity)
        {
            _context = context;
            _toFolder = toFolder;
            _toFolderEntity = toFolderEntity;
        }

        public Folder GetFolderTree(Guid root)
        {
            var folder = _context.Folders.Single(x => x.Id == root);
            return _context.GetTree(folder).Map(_toFolder);
        }

        public Guid CreateTree(Folder root)
        {
            var rootEntity = root.Map(_toFolderEntity);

            _context.Folders.Add(rootEntity);
            _context.SaveChanges();

            return rootEntity.Id;
        }

        public void UpdateRootFolder(Folder updatedTree)
        {
            var rootEntity = _context.Folders.Single(x => x.Id == updatedTree.Id);
            var originalTree = _context.GetTree(rootEntity);

            originalTree.Name = updatedTree.Name;
            UpdateNodeByName(originalTree, updatedTree);
        }

        private void UpdateNodeByName(FolderEntity originalTree, Folder updatedTree)
        {
            originalTree.LastModified = updatedTree.LastModified;

            foreach (var original in originalTree.SubFolders)
            {
                var updated = updatedTree.SubFolders.SingleOrDefault(x => x.Name == original.Name);
                if (updated == null)
                {
                    //deleted
                    RemoveNode(_context.Folders.Remove(original));
                }
                else
                {
                    //modified
                    UpdateNodeByName(original, updated);
                }
            }

            //add
            foreach (var subFolder in updatedTree.SubFolders.Where(x => originalTree.SubFolders.All(y => !y.Name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))))
            {
                originalTree.SubFolders.Add(subFolder.Map(_toFolderEntity));
            }
        }

        private void RemoveNode(FolderEntity remove)
        {
            _context.Folders.Remove(remove);
            foreach (var folder in remove.SubFolders)
            {
                RemoveNode(folder);
            }
        }
    }
}
