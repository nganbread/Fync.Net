using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Fync.Service;
using Fync.Service.Models;

namespace Fync.Api.Controllers
{
    [Authorize]
    public class FileController : ApiController
    {
        private readonly ISymbolicFileService _symbolicFileService;

        public FileController(ISymbolicFileService symbolicFileService)
        {
            _symbolicFileService = symbolicFileService;
        }

        public IList<SymbolicFile> Get(Guid folderId)
        {
            return _symbolicFileService.GetFilesInFolder(folderId);
        }

        public SymbolicFile Get(Guid folderId, string fileName)
        {
            var file = _symbolicFileService.GetFileOrDefault(folderId, fileName);
            if(file == null) throw new HttpResponseException(HttpStatusCode.BadRequest);

            return file;
        }

        public HttpResponseMessage Delete(Guid folderId, DeletedSymbolicFile deletedSymbolicFile)
        {
            _symbolicFileService.DeleteSymbolicFileFromFolder(folderId, deletedSymbolicFile.FileName);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Put(Guid folderId, NewSymbolicFile symbolicFile)
        {
            _symbolicFileService.AddFileToFolder(folderId, symbolicFile);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Post(Guid folderId)
        {
            var keys = HttpContext.Current.Request.Files.AllKeys.Distinct().ToList();
            if(keys.Count() != 1) return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var httpFile = HttpContext.Current.Request.Files[keys.Single()];

            _symbolicFileService.CreateFile(httpFile.InputStream, folderId, httpFile.FileName);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}