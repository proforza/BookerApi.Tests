using System.Text.Json;
using System.Text;
using System;
using BookerApi.Tests.Classes;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookerApi.Tests.Helpers
{
    // This helper doesn't used anywhere
    public static class TokenHelper
    {
        public static async Task<string> GetToken()
        {
            string token = String.Empty;
            HttpClient httpClient = BaseClient.GetHttpClient();
            string contentBody = JsonSerializer.Serialize(new { username = "admin", password = "password123" }); // this credentials should be somewhere in appsettings.config, or better in secrets
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync("auth", new StringContent(contentBody, Encoding.UTF8, "application/json"));
                var responseBody = await response.Content.ReadAsStringAsync();
                token = JsonSerializer.Deserialize<TokenEntity>(responseBody).token;
            }
            catch (Exception)
            {
                // maybe some logging here
            }

            return token;
        }
    }
}
