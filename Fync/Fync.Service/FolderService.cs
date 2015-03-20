using System;
using System.Data.Entity;
using System.Linq;
using Fync.Common;
using Fync.Common.Models;
using Fync.Data;
using Fync.Service.Models.Data;

namespace Fync.Service
{

    internal class FolderService : IFolderService
    {
        private readonly IContext _context;
        private readonly Func<FolderEntity, FolderWithChildren> _toFolder;
        private readonly Func<FolderEntity, int, FolderWithChildren> _toFolderWithDepth;
        private readonly Func<NewFolder, FolderEntity> _toFolderEntity;
        private readonly ICurrentUser _currentUser;
        private readonly ISymbolicFileService _symbolicFileService;
        private readonly Func<FolderEntity, int, FolderWithParentAndChildren> _toFolderWithParentAndChildren;

        public FolderService(
            IContext context, 
            Func<FolderEntity, FolderWithChildren> toFolder, 
            Func<FolderEntity, int, FolderWithChildren> toFolderWithDepth, 
            Func<NewFolder, FolderEntity> toFolderEntity, 
            ICurrentUser currentUser,
            ISymbolicFileService symbolicFileService,
            Func<FolderEntity, int, FolderWithParentAndChildren> toFolderWithParentAndChildren)
        {
            _context = context;
            _toFolder = toFolder;
            _toFolderWithDepth = toFolderWithDepth;
            _toFolderEntity = toFolderEntity;
            _currentUser = currentUser;
            _symbolicFileService = symbolicFileService;
            _toFolderWithParentAndChildren = toFolderWithParentAndChildren;
        }

        public FolderWithChildren CreateFolder(string[] pathComponents, DateTime createDate)
        {
            var folderName = pathComponents.Last();
            //Kinda inefficient
            var folder = GetFolderFromPath(pathComponents.Take(pathComponents.Length - 1).ToArray());

            return CreateFolder(folder.Id, folderName, createDate);
        }

        public FolderWithChildren CreateFolder(Guid folderId, string folderName, DateTime createDate)
        {
            var parent = _context.Folders.Single(x => x.Id == folderId);
            if (parent.Owner.Id != _currentUser.UserId) throw new Exception();

            var existing =
                parent.SubFolders.SingleOrDefault(x => x.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase));
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

        public FolderWithChildren DeleteFolder(Guid folderId, DateTime dateDeleted)
        {
            var folder = _context.Folders.Single(x => x.Id == folderId);
            if(folder.Parent == null) throw new Exception(); //cant delete a root folder
            if(folder.Owner != _currentUser.User) throw new Exception();
            RemoveNode(folder, dateDeleted);

            _context.SaveChanges();

            return folder.Map(_toFolder);
        }

        public FolderWithChildren GetFullTree()
        {
            return _context.GetTree(_currentUser.UserId).Map(_toFolder);
        }

        public FolderWithChildren GetFolder(Guid id)
        {
            var folder = _context.Folders.Include(x => x.SubFolders).Single(x => x.Id == id);
            return folder.Map(x => _toFolderWithDepth(x, 1)); //only enumerate to a depth of one level
        }

        public FolderWithParentAndChildren GetFolderFromPath(string[] pathComponents)
        {
            //Root folder is Fync
            if (pathComponents.First() != "Fync") return null;

            var folder = _context.GetFolderFromPath(_currentUser.UserId, pathComponents);
            return folder == null
            ? null
            : folder.Map(x => _toFolderWithParentAndChildren(x, 1));
        }

        public FolderWithChildren GetFullTree(Guid root)
        {
            return _context.GetTree(_currentUser.UserId, root).Map(_toFolder);
        }

        public void UpdateRootFolder(NewFolder updatedRootFolder, DateTime updateDate)
        {
            var rootFolder = _context.GetTree(_currentUser.UserId);

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
            folderToRemove.RecursivelyDo(x =>
            {
                x.Deleted = true;
                x.ModifiedDate = dateDeleted;
            }, x => x.SubFolders);
        }
    }
}
