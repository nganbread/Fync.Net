using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fync.Common.Models;
using Fync.Service;

namespace Fync.Api.Controllers
{
    [Authorize]
    public class FolderController : ApiController
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        public Folder Get(Guid? id = null)
        {
            return id.HasValue
                ? _folderService.GetFullTree(id.Value)
                : _folderService.GetFullTree();
        }

        public Folder Post(Guid folderId, NewFolder newFolder)
        {
            return _folderService.CreateFolder(folderId, newFolder.Name, DateTime.UtcNow);
        }

        public HttpResponseMessage Delete(Guid folderId)
        {
            _folderService.DeleteFolder(folderId, DateTime.UtcNow);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Put(NewFolder root)
        {
            if(root == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _folderService.UpdateRootFolder(root, DateTime.UtcNow);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
