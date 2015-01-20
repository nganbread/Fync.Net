using System;
using System.Web;
using System.Web.Http;
using Fync.Service;
using Fync.Service.Models;

namespace Fync.Api.Controllers
{
    public class FolderController : ApiController
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        public Folder Get(Guid id)
        {
            return _folderService.GetFolderTree(id);
        }

        public void Put(Folder root)
        {
            if(root.Id == Guid.Empty) throw new HttpException();
            _folderService.UpdateRootFolder(root);
        }

        public Guid Post(Folder root)
        {
            return _folderService.CreateTree(root);
        }
    }
}
