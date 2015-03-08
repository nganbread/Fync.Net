using System.Linq;
using System.Web.Http;
using Fync.Api.ModelBinders;
using Fync.Common;
using Fync.Common.Models;
using Fync.Service;

namespace Fync.Api.Controllers
{
    public class FyncController : ApiController
    {
        private readonly IFolderService _folderService;

        public FyncController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        public FolderWithParentAndChildren Get([SlashSeparatedArray]string[] pathComponents)
        {
            return _folderService.GetFolderFromPath("Fync".Concatenate(pathComponents).ToArray());
        }
    }
}