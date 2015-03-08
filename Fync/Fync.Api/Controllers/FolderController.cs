using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Fync.Api.ModelBinders;
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

        public FolderWithChildren Get(Guid? id = null)
        {
            return id.HasValue
                ? _folderService.GetFullTree(id.Value)
                : _folderService.GetFullTree();
        }

        public FolderWithChildren Post([SlashSeparatedArrayAttribute]string[] pathComponents, NewFolder newFolder)
        {
            return _folderService.CreateFolder(pathComponents, DateTime.UtcNow);
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
