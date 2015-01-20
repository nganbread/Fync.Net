using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Fync.Service;
using Fync.Service.Models;

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

        public HttpResponseMessage Put(Folder root)
        {
            if(root == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _folderService.UpdateRootFolder(root);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
