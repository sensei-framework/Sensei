using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sensei.AspNet.Tests.Utils
{
    public static class HttpClientExtension
    {
        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client, string path, object data)
        {
            return await client.PostAsync(path,
                new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
        }

        public static async Task<HttpResponseMessage> PutAsync(this HttpClient client, string path, object data)
        {
            return await client.PutAsync(path,
                new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
        }

        public static async Task<T> PostAsync<T>(this HttpClient client, string path, object data)
        {
            var response = await PostAsync(client, path, data);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> PutAsync<T>(this HttpClient client, string path, object data)
        {
            var response = await PutAsync(client, path, data);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> GetAsync<T>(this HttpClient client, string path)
        {
            var response = await client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}