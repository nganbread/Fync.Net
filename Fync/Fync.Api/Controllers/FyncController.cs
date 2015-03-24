using System;
using System.Web;
using System.Web.Http;
using Fync.Service;

namespace Fync.Api.Controllers
{
    [Authorize]
    public class FyncController : ApiController
    {
        private readonly IFolderService _folderService;
        private readonly ISymbolicFileService _symbolicFileService;

        public FyncController(IFolderService folderService, ISymbolicFileService symbolicFileService)
        {
            _folderService = folderService;
            _symbolicFileService = symbolicFileService;
        }

        public object Get(Guid id)
        {
            var folder = _folderService.GetFolderWithParents(id);
            if (folder == null) throw new HttpException(404, "Folder Not Found");
            var files = _symbolicFileService.GetFilesInFolder(folder.Id);

            return new
            {
                files = files,
                folders = folder.SubFolders,
                parent = folder.Parent,
                id = folder.Id,
                name = folder.Name,
                modifiedDate = folder.ModifiedDate,
                deleted = folder.Deleted
            };
        }
    }
}