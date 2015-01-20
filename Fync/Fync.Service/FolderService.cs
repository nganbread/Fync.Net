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
        private readonly ICurrentUser _currentUser;

        public FolderService(IContext context, Func<FolderEntity, Folder> toFolder, Func<Folder, FolderEntity> toFolderEntity, ICurrentUser currentUser)
        {
            _context = context;
            _toFolder = toFolder;
            _toFolderEntity = toFolderEntity;
            _currentUser = currentUser;
        }

        public Folder GetFullTree()
        {
            return _currentUser.User.RootFolder == null 
                ? null
                : GetFullTree(_currentUser.User.RootFolder.Id);   
        }

        public Folder GetFullTree(Guid root)
        {
            return _context.GetTree(root).Map(_toFolder);
        }

        public void UpdateRootFolder(Folder updatedRootFolder)
        {
            if (_currentUser.User.RootFolder == null)
            {
                _currentUser.User.RootFolder = updatedRootFolder.Map(_toFolderEntity);
                _context.SaveChanges();
                return;
            }
            var rootFolder = _context.GetTree(_currentUser.User.RootFolder.Id);

            rootFolder.Name = updatedRootFolder.Name;
            UpdateNodeByName(rootFolder, updatedRootFolder);

            _context.SaveChanges();
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
