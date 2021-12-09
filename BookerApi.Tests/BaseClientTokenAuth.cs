using BookerApi.Tests.Classes;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BookerApi.Tests
{
    // A singletone httpClient here with Cookie token auth
    // Creating a token can be implemented right here, but I decided to create a test for it, where the token is being set, and run it before others
    public class BaseClientTokenAuth
    {
        private static readonly HttpClient HttpClient;
        private static readonly TokenEntity Token;

        static BaseClientTokenAuth()
        {
            HttpClient = new();
            HttpClient.BaseAddress = new Uri("https://restful-booker.herokuapp.com/");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Token = new();
        }

        public static HttpClient GetHttpClient()
        {
            return HttpClient;
        }
        public static string GetToken()
        {
            return Token.token;
        }
        public static void SetToken(string token)
        {
            Token.token = token;
        }
    }
}
