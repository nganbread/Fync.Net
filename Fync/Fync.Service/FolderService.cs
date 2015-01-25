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

        public void UpdateRootFolder(NewFolder updatedRootFolder)
        {
            if (_currentUser.User.RootFolder == null)
            {
                var root = updatedRootFolder.Map(_toFolderEntity);
                _context.Folders.Add(root);
                _currentUser.User.RootFolder = root;
                _context.SaveChanges();
                return;
            }
            var rootFolder = _context.GetTree(_currentUser.User.RootFolder.Id);

            UpdateNodeByName(rootFolder, updatedRootFolder);

            _context.SaveChanges();
        }

        private void UpdateNodeByName(FolderEntity originalTree, NewFolder updatedTree)
        {
            foreach (var original in originalTree.SubFolders.ToList())
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
                var folderEntity = subFolder.Map(_toFolderEntity);
                folderEntity.DateCreated = DateTime.UtcNow;
                originalTree.SubFolders.Add(folderEntity);
            }
        }

        private void RemoveNode(FolderEntity folderToRemove)
        {
            _symbolicFileService.DeleteSymbolicFilesFromFolder(folderToRemove.Id);
            _context.Folders.Remove(folderToRemove);
            foreach (var folder in folderToRemove.SubFolders.ToList())
            {
                RemoveNode(folder);
            }
        }
    }
}
