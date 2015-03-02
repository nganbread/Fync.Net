using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fync.Service;
using Fync.Web.ModelBinders;
using Fync.Web.Models;

namespace Fync.Web.Controllers
{
    public class FyncController : Controller
    {
        private readonly IFolderService _folderService;
        private readonly ISymbolicFileService _symbolicFileService;

        public FyncController(IFolderService folderService, ISymbolicFileService symbolicFileService)
        {
            _folderService = folderService;
            _symbolicFileService = symbolicFileService;
        }

        public ActionResult Index([ModelBinder(typeof(SlashSeparatedArray))]string[] pathComponents)
        {
            var folder = _folderService.GetFolderFromPath(pathComponents);
            if(folder == null) throw new HttpException(404, "Folder Not Found");
            var files = _symbolicFileService.GetFilesInFolder(folder.Id);

            return View(new FyncModel
            {
                FolderWithParentAndChildren = folder,
                Files = files
            });
        }
    }
}
