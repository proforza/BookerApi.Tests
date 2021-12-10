using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Text.Json;
using System.Text;
using System;
using BookerApi.Tests.Classes;
using System.Collections.Generic;

namespace BookerApi.Tests.AuthTests
{
    [TestFixture, Order(3), Description("Tests to ensure that token auth is working properly")]
    [Author("Aleksey Kulikov", "proforza@ya.ru")]
    public class TokenAuthTests
    {
        private List<BookingRoot> bookingIds;

        [Test, Order(1)]
        public async Task BookingIds_ShouldBe_Retrieved_And_ShouldNotBe_Empty()
        {
            // Arrange
            HttpClient httpClient = BaseClientTokenAuth.GetHttpClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync("booking");
            bookingIds = JsonSerializer.Deserialize<List<BookingRoot>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(bookingIds.Count > 0);
        }

        [Test, Order(2)]
        public async Task RandomBooking_ShouldBe_Patched_Using_TokenAuth()
        {
            // Arrange
            if (String.IsNullOrEmpty(BaseClientTokenAuth.GetToken()))
                throw new Exception("Try to set auth token first.");

            HttpClient httpClient = BaseClientTokenAuth.GetHttpClient();
            Random rnd = new();
            int randomId = bookingIds[rnd.Next(0, bookingIds.Count - 1)].bookingid;
            var request = new HttpRequestMessage(HttpMethod.Patch, $"booking/{randomId}");
            request.Headers.Add("Cookie", $"token={BaseClientTokenAuth.GetToken()}"); // using "Cookie: token=123pwd" header
            request.Content = new StringContent(JsonSerializer.Serialize( new { firstname = "Giovanni" } ), Encoding.UTF8, "application/json"); // changing the name

            // Act
            HttpResponseMessage response = await httpClient.SendAsync(request);
            var randomPatchedBooking = JsonSerializer.Deserialize<Booking>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(randomPatchedBooking.firstname == "Giovanni");
        }
    }
}
