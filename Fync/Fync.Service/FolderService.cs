using System;
using System.Linq;
using Fync.Common;
using Fync.Common.Models;
using Fync.Data;
using Fync.Service.Models;
using Fync.Service.Models.Data;

namespace Fync.Service
{

    internal class FolderService : IFolderService
    {
        private readonly IContext _context;
        private readonly Func<FolderEntity, Folder> _toFolder;
        private readonly Func<NewFolder, FolderEntity> _toFolderEntity;
        private readonly ICurrentUser _currentUser;
        private readonly ISymbolicFileService _symbolicFileService;

        public FolderService(IContext context, Func<FolderEntity, Folder> toFolder, Func<NewFolder, FolderEntity> toFolderEntity, ICurrentUser currentUser, ISymbolicFileService symbolicFileService)
        {
            _context = context;
            _toFolder = toFolder;
            _toFolderEntity = toFolderEntity;
            _currentUser = currentUser;
            _symbolicFileService = symbolicFileService;
        }

        public Folder CreateFolder(Guid folderId, string folderName, DateTime createDate)
        {
            var parent = _context.Folders.Single(x => x.Id == folderId);
            if(parent.Owner.Id != _currentUser.User.Id) throw new Exception();

            var existing = parent.SubFolders.SingleOrDefault(x => x.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase));
            if (existing != null)
            {
                existing.Name = folderName;
                existing.Deleted = false;
                existing.ModifiedDate = createDate;
                _context.SaveChanges();
                return existing.Map(_toFolder);
            }
            else
            {
                var newFolder = new FolderEntity
                {
                    Name = folderName,
                    Owner = _currentUser.User,
                    ModifiedDate = createDate
                };
                parent.SubFolders.Add(newFolder);
                _context.SaveChanges();
                return newFolder.Map(_toFolder);
            }
        }

        public void DeleteFolder(Guid folderId, DateTime dateDeleted)
        {
            var folder = _context.Folders.Single(x => x.Id == folderId);
            if(folder.Parent == null) throw new Exception(); //cant delete a root folder
            if(folder.Owner != _currentUser.User) throw new Exception();
            RemoveNode(folder, dateDeleted);
        }

        public Folder GetFullTree()
        {
            return _context.GetTree(_currentUser.User.Id).Map(_toFolder);
        }

        public Folder GetFullTree(Guid root)
        {
            return _context.GetTree(root).Map(_toFolder);
        }

        public void UpdateRootFolder(NewFolder updatedRootFolder, DateTime updateDate)
        {
            var rootFolder = _context.GetTree(_currentUser.User.Id);

            UpdateNodeByName(rootFolder, updatedRootFolder, updateDate);

            _context.SaveChanges();
        }

        private void UpdateNodeByName(FolderEntity originalTree, NewFolder updatedTree, DateTime updateDate)
        {
            foreach (var original in originalTree.SubFolders.ToList())
            {
                var updated = updatedTree.SubFolders.SingleOrDefault(x => x.Name == original.Name);
                if (updated == null)
                {
                    //deleted
                    RemoveNode(_context.Folders.Remove(original), updateDate);
                }
                else
                {
                    //modified
                    UpdateNodeByName(original, updated, updateDate);
                }
            }

            //add
            foreach (var subFolder in updatedTree.SubFolders.Where(x => originalTree.SubFolders.All(y => !y.Name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))))
            {
                var folderEntity = subFolder.Map(_toFolderEntity);
                folderEntity.Owner = _currentUser.User;
                folderEntity.ModifiedDate = updateDate;
                originalTree.SubFolders.Add(folderEntity);
            }
        }

        private void RemoveNode(FolderEntity folderToRemove, DateTime dateDeleted)
        {
            _symbolicFileService.DeleteSymbolicFilesFromFolder(folderToRemove.Id, dateDeleted);
            folderToRemove.RecursivelySet(x =>
            {
                x.Deleted = true;
                x.ModifiedDate = dateDeleted;
            }, x => x.SubFolders);
        }
    }
}
