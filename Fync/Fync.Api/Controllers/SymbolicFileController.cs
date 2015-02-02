using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Fync.Common.Models;
using Fync.Service;
using Fync.Service.Models;
using Newtonsoft.Json;

namespace Fync.Api.Controllers
{
    [Authorize]
    public class SymbolicFileController : ApiController
    {
        private readonly ISymbolicFileService _symbolicFileService;

        public SymbolicFileController(ISymbolicFileService symbolicFileService)
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
            _symbolicFileService.DeleteSymbolicFileFromFolder(folderId, deletedSymbolicFile.FileName, DateTime.UtcNow);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Put(Guid folderId, NewSymbolicFile symbolicFile)
        {
            _symbolicFileService.AddSymbolicFileToFolder(folderId, symbolicFile, DateTime.UtcNow);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> Post(Guid folderId)
        {
            try
            {
            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.Contents.Count != 2) return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var stream = await provider.Contents.Single(x => x.Headers.ContentType.MediaType != "application/file").ReadAsStreamAsync();
            var modelJson = await provider.Contents.Single(x => x.Headers.ContentType.MediaType == "application/json").ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<NewFile>(modelJson);

                _symbolicFileService.CreateSymbolicFile(stream, folderId, model.Name, DateTime.UtcNow);

            }
            catch (Exception e)
            {
                
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}