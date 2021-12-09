using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BookerApi.Tests
{
    [TestFixture, Order(1)]
    [Author("Aleksey Kulikov", "proforza@ya.ru")]
    public class HealthCheckTests
    {
        // perfectly this test must be the first one, to ensure that API is available
        // also this healthcheck can be implemented as a helper method, to use it before every single test
        [Test]
        public async Task HealthCheckStatus_ShouldBe_Ok() 
        {
            // Arrange
            HttpClient httpClient = BaseClient.GetHttpClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync("ping");

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
