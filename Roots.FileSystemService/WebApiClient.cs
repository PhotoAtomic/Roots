using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        
    }
}
