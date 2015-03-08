using System.IO;
using System.Threading.Tasks;

namespace Fync.Client.Web
{
    internal interface IHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
        Task<T> GetAsync<T>(string requestUri, params object[] format);

        Task PutAsync(string requestUri, object data);
        Task<T> PutAsync<T>(string requestUri, object data);

        Task<Stream> GetStreamAsync(string requestUri);
        Task<Stream> GetStreamAsync(string requestUri, params object[] format);

        Task PostStreamAsync(Stream fileStream, object model, string requestUri, params object[] format);
        Task<T> PostStreamAsync<T>(Stream fileStream, object model, string requestUri, params object[] format);
        Task<T> PostAsync<T>(string requestUri, object data);

        Task DeleteAsync(string requestUri, params object[] format);
    }
}