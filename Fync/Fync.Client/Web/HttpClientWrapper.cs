using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fync.Common;
using Fync.Common.Models;
using Newtonsoft.Json;

namespace Fync.Client.Web
{
    internal class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(IClientConfiguration clientConfiguration)
        {
            _httpClient = new HttpClient(new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                BaseAddress = clientConfiguration.BaseUri,
            };
            var authenticationDetails = new AuthenticationDetails
            {
                EmailAddress = clientConfiguration.EmailAddress,
                Password = clientConfiguration.Password
            };
            var _ = _httpClient.PostAsJsonAsync(@"Authenticate", authenticationDetails).Result;
        }
        
        public async Task<T> GetAsync<T>(string requestUri)
        {
            var httpResponse = await _httpClient.GetAsync(requestUri);
            return await HandleResponse<T>(httpResponse);
        }

        public async Task<T> PutAsync<T>(string requestUri, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(requestUri, data);
            return await HandleResponse<T>(response);
        }

        public async Task<Stream> GetStreamAsync(string requestUri)
        {
            return await _httpClient.GetStreamAsync(requestUri);
        }

        public Task<Stream> GetStreamAsync(string requestUri, params object[] format)
        {
            return GetStreamAsync(requestUri.FormatWith(format));
        }

        public async Task PostStreamAsync(Stream fileStream, object model, string requestUri, params object[] format)
        {
            var content = new MultipartFormDataContent
            {
                new StreamContent(fileStream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/file")}
                },
                new StringContent(JsonConvert.SerializeObject(model))
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json")}
                }
            };
            var response = await _httpClient.PostAsync(requestUri.FormatWith(format), content);
            HandleResponse(response);
        }

        public async Task<T> PostStreamAsync<T>(Stream fileStream, object model, string requestUri, params object[] format)
        {
            var content = new MultipartFormDataContent
            {
                new StreamContent(fileStream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/file")}
                },
                new StringContent(JsonConvert.SerializeObject(model))
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json")}
                }
            };
            var response = await _httpClient.PostAsync(requestUri.FormatWith(format), content);
            return await HandleResponse<T>(response);
        }

        public async Task<T> PostAsync<T>(string requestUri, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(requestUri, data);
            return await HandleResponse<T>(response);
        }

        private void HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }    
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(JsonConvert.SerializeObject(response));
            }
            return await response.Content.ReadAsAsync<T>();
        }

        public async Task DeleteAsync(string requestUri, params object[] format)
        {
            var response = await _httpClient.DeleteAsync(requestUri.FormatWith(format));
            HandleResponse(response);
        }

        public Task<T> GetAsync<T>(string requestUri, params object[] format)
        {
            return GetAsync<T>(requestUri.FormatWith(format));
        }

        public async Task PutAsync(string requestUri, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(requestUri, data);
            HandleResponse(response);
        }

        public async Task PostAsync(string requestUri, object obj)
        {
            var response = await _httpClient.PostAsJsonAsync(requestUri, obj);
            HandleResponse(response);
        }
    }
}