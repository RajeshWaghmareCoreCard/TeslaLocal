using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreCard.Tesla.Utilities
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> PostJsonAsync<TResponse, TRequest>(this HttpClient httpClient, TRequest request, string endpoint)
        {
            try
            {
                var requestJson = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(requestJson, System.Text.UTF8Encoding.UTF8, "application/json");
                var httpResponse = await httpClient.PostAsync($"{httpClient.BaseAddress}{endpoint}", httpContent);
                //TODO - test EnsureSuccessStatusCode
                // httpResponse.EnsureSuccessStatusCode();
                var response = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(response);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return default(TResponse);
            }
        }
    }
}
