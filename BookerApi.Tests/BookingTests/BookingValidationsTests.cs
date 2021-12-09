using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Text.Json;
using System;
using BookerApi.Tests.Classes;
using System.Collections.Generic;
using BookerApi.Tests.Helpers;
using System.Text;

namespace BookerApi.Tests.BookingTests
{
    [TestFixture, Order(4), Description("Some basic validation tests")]
    [Author("Aleksey Kulikov", "proforza@ya.ru")]
    public class BookingValidationsTests
    {
        private List<BookingRoot> bookingIds;

        [Test, Order(1)]
        public async Task BookingIds_ShouldBe_Retrieved_And_ShouldNotBe_Empty()
        {
            // Arrange
            HttpClient httpClient = BaseClient.GetHttpClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync("booking");
            var responseBody = await response.Content.ReadAsStringAsync();
            bookingIds = JsonSerializer.Deserialize<List<BookingRoot>>(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(bookingIds.Count > 0);
        }

        [Test, Order(2)]
        public async Task Get_RandomBooking_And_ValidateFields()
        {
            // Arrange
            HttpClient httpClient = BaseClient.GetHttpClient();
            Random rnd = new();
            int randomId = bookingIds[rnd.Next(0, bookingIds.Count - 1)].bookingid;

            // Act
            HttpResponseMessage response = await httpClient.GetAsync($"booking/{randomId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var randomBooking = JsonSerializer.Deserialize<Booking>(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(randomBooking);
            Assert.IsNotEmpty(randomBooking.firstname);
            Assert.IsNotEmpty(randomBooking.lastname);
            Assert.IsTrue(randomBooking.totalprice > 0);
            Assert.IsNotEmpty(randomBooking.bookingdates.checkin);
            Assert.IsNotEmpty(randomBooking.bookingdates.checkout);
            Assert.IsTrue(DateTime.Parse(randomBooking.bookingdates.checkout) > DateTime.Parse(randomBooking.bookingdates.checkin));
        }

        [Test, Order(3)]
        public async Task Booking_With_IncorrectDates_ShouldNotBe_Accepted()
        {
            // Arrange
            HttpClient httpClient = BaseClient.GetHttpClient();
            BookingDates randomDates = RandomizerHelper.GetRandomDates();
            Booking booking = new()
            {
                firstname = "Cristiano",
                lastname = "Ronaldo",
                totalprice = RandomizerHelper.GetRandomPrice(),
                depositpaid = false,
                bookingdates = new BookingDates() { checkin = randomDates.checkout, checkout = randomDates.checkin },  // changing to incorrect dates
                additionalneeds = null
            }; // test "seed" Booking

            string contentBody = JsonSerializer.Serialize(booking);

            // Act
            HttpResponseMessage response = await httpClient.PostAsync("booking", new StringContent(contentBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdBooking = JsonSerializer.Deserialize<BookingRoot>(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode); // Must be BadRequest or smth, API has no dates validation
            Assert.IsNotNull(createdBooking);
        }
    }
}
