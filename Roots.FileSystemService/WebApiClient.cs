using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Roots.FileSystemService
{
    public class WebApiClient
    {
        private Uri serverUri;
        
        public WebApiClient(string serverUri): this(new Uri(serverUri))
        {            
        }

        public WebApiClient(Uri serverUri)
        {
            this.serverUri = serverUri;
        }

        public async Task<HttpResponseMessage> PostAsync(string method, params object[] contentDto)
        {
            HttpContent content = MakeJson(contentDto);
            return await MakeClient().PostAsync(method,content);            
        }

        public async Task<HttpResponseMessage> DeleteAsync(string method, object requestArgs)
        {
            string queryString = MakeQueryString(method, requestArgs);

            return await MakeClient().DeleteAsync(queryString);
        }

        public async Task<HttpResponseMessage> PutAsync(string method, object requestArgs, params object[] contentDto)
        {
            string queryString = MakeQueryString(method, requestArgs);

            HttpContent content = MakeJson(contentDto);

            return await MakeClient().PutAsync(queryString, content);
        }

        private static string MakeQueryString(string method, object requestArgs)
        {
            var type = requestArgs.GetType();
            var properties = type.GetProperties();
            var listOfArgs = Enumerable.Zip(
                properties.Select(x => x.Name),
                properties.Select(x => x.GetValue(requestArgs)),
                (k, v) => new KeyValuePair<string, object>(k, v));

            var stringsOfArgs = listOfArgs
                .Where(x => x.Value != null)
                .Select(x =>
                    String.Format("{0}={1}", x.Key, HttpUtility.UrlPathEncode(x.Value.ToString())));

            string queryString;
            if (stringsOfArgs.Count() > 0)
            {
                queryString = string.Format("{0}?{1}", method, string.Join("&", stringsOfArgs));
            }
            else
            {
                queryString = method;
            }
            return queryString;
        }

        public async Task<T> GetAsync<T>(string method, object requestArgs)
        {
            string queryString = MakeQueryString(method, requestArgs);
            var result =  await MakeClient().GetAsync(queryString);
            return await MakeObject<T>(result.Content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string method)
        {            
            return await MakeClient().DeleteAsync(method);
        }

        private HttpClient MakeClient()
        {
            
            HttpClient client = new HttpClient();
            client.BaseAddress = serverUri;

            return client;
        }

        private static HttpContent MakeJson(object[] contentDto)
        {
            string json;
            if (contentDto == null || contentDto.Length == 0)
            {
                json = string.Empty;
            }
            else if (contentDto.Length == 1)
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(contentDto[0]);
            }
            else
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(contentDto);
            }

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        private static async Task<T> MakeObject<T>(HttpContent content)
        {
            if (content.Headers.ContentType.MediaType != "application/json") return default(T);
            //string json;
            //if (contentDto == null || contentDto.Length == 0)
            //{
            //    json = string.Empty;
            //}
            //else if (contentDto.Length == 1)
            //{
            //    json = Newtonsoft.Json.JsonConvert.SerializeObject(contentDto[0]);
            //}
            //else
            //{
            //    json = Newtonsoft.Json.JsonConvert.SerializeObject(contentDto);
            //}

            //HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            //return content;
            var body = await content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(body);
        }
        
    }
}
