using System;
using System.Web.Mvc;
using Fync.Service;
using Fync.Web.Models;

namespace Fync.Web.Controllers
{
    public class FolderController : Controller
    {
        private readonly IFolderService _folderService;
        private readonly ISymbolicFileService _symbolicFileService;
        private readonly ICurrentUser _currentUser;

        public FolderController(IFolderService folderService, ISymbolicFileService symbolicFileService, ICurrentUser currentUser)
        {
            _folderService = folderService;
            _symbolicFileService = symbolicFileService;
            _currentUser = currentUser;
        }

        public ActionResult Index(Guid? id)
        {
            id = id ?? _currentUser.User.RootFolderId;

            var files = _symbolicFileService.GetFilesInFolder(id.Value);
            var folder = _folderService.GetFolder(id.Value);

            return View(new FolderModel
            {
                Folder = folder,
                Files = files
            });
        }
    }
}