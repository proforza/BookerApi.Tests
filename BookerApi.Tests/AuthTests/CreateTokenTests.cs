using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Text.Json;
using System.Text;
using System;
using BookerApi.Tests.Classes;

namespace BookerApi.Tests
{
    [TestFixture, Order(2), Description("Checking the possibility of creating auth token")]
    [Author("Aleksey Kulikov", "proforza@ya.ru")]
    public class CreateTokenTests
    {
        [Test]
        public async Task AuthToken_ShouldBe_Created()
        {
            // Arrange
            HttpClient httpClient = BaseClientTokenAuth.GetHttpClient();
            string contentBody = JsonSerializer.Serialize(new { username = "admin", password = "password123" }); // this credentials should be somewhere in appsettings.config, or better in secrets

            // Act
            HttpResponseMessage response = await httpClient.PostAsync("auth", new StringContent(contentBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            BaseClientTokenAuth.SetToken(JsonSerializer.Deserialize<TokenEntity>(responseBody).token); // setting the token to BaseClientTokenAuth

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsFalse(String.IsNullOrEmpty(BaseClientTokenAuth.GetToken()));
        }
    }
}
