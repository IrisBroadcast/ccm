using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace CCM.Core.Helpers
{
    public class HttpService
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static async Task<T> GetAsync<T>(Uri url) where T : class
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    log.Warn("Request to {0} failed.", url);
                    return null;
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(Uri url, object data = null)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpContent content = null;
                    if (data != null)
                    {
                        var s = JsonConvert.SerializeObject(data);
                        content = new StringContent(s, Encoding.UTF8, "application/json");
                    }
                    return await client.PostAsync(url, content);
                }
                catch (Exception)
                {
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
                }
            }
        }

    }
}