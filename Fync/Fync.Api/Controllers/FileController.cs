using System.Web.Http;
using Fync.Service;

namespace Fync.Api.Controllers
{
    [Authorize]
    public class FileController : ApiController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public bool Get(string hash)
        {
            return _fileService.FileExists(hash);
        }
    }
}