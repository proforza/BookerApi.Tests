using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BookerApi.Tests
{
    public class BaseClient
    {
        // A singletone httpClient here with basic auth headers
        private static readonly HttpClient HttpClient;
        static BaseClient()
        {
            HttpClient = new();
            HttpClient.BaseAddress = new Uri("https://restful-booker.herokuapp.com/");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // this credentials should be somewhere in appsettings.config, or better in secrets
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("admin:password123")));
        }
        public static HttpClient GetHttpClient()
        {
            return HttpClient;
        }
    }
}
