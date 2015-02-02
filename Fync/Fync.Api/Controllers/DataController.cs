using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Fync.Service;

namespace Fync.Api.Controllers
{
    public class DataController : ApiController
    {
        private readonly ISymbolicFileService _symbolicFileService;

        public DataController(ISymbolicFileService symbolicFileService)
        {
            _symbolicFileService = symbolicFileService;
        }

        public HttpResponseMessage Get(Guid folderId, string fileName)
        {
            var file = _symbolicFileService.GetFile(folderId, fileName);
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(file.Stream)
                    {
                        Headers =
                        {
                            ContentLength = file.LengthInBytes,
                            ContentType = new MediaTypeHeaderValue(file.ContentType),
                            ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = file.FileName,
                                Size = file.LengthInBytes
                            }
                        }
                    }
            };

            return message;
        }
    }
}
