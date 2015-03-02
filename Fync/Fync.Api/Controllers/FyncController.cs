using System.Web.Http;
using System.Web.Http.ModelBinding;
using Fync.Api.ModelBinders;
using Fync.Common.Models;
using Fync.Service;

namespace Fync.Api.Controllers
{
    [AllowAnonymous]
    public class FyncController : ApiController
    {
        private readonly IFolderService _folderService;

        public FyncController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        public FolderWithChildren Get([ModelBinder(typeof(SlashSeparatedArray))]string[] pathComponents)
        {
            return _folderService.GetFolderFromPath(pathComponents);
        }
    }
}