using System;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using Fync.Service;

namespace Fync.Web.Controllers
{
    public class DataController : Controller
    {
        private readonly ISymbolicFileService _symbolicFileService;

        public DataController(ISymbolicFileService symbolicFileService)
        {
            _symbolicFileService = symbolicFileService;
        }

        public ActionResult Index(Guid id, string fileName)
        {

            var file = _symbolicFileService.GetFile(id, fileName);
            return File(file.Stream, file.ContentType, file.FileName);
        }
    }
}