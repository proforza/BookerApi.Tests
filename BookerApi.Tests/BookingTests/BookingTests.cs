using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Text.Json;
using System.Text;
using System;
using BookerApi.Tests.Classes;
using System.Collections.Generic;
using System.Linq;
using BookerApi.Tests.Helpers;

namespace BookerApi.Tests.BookingTests
{
    [TestFixture, Description("Full lifecycle of a Booking entity. Create -> update -> patch -> delete")]
    [Author("Aleksey Kulikov", "proforza@ya.ru")]
    public class BookingTests
    {
        private BookingRoot CreatedBooking;
        private readonly HttpClient httpClient = BaseClient.GetHttpClient();

        [Test, Order(1)]
        public async Task Booking_ShouldBe_Created()
        {
            // Arrange
            BookingDates randomDates = RandomizerHelper.GetRandomDates();

            Booking booking = new() { 
                firstname = "Aleksey",
                lastname = "Kulikov",
                totalprice = RandomizerHelper.GetRandomPrice(),
                depositpaid = false,
                bookingdates = new BookingDates() { checkin = randomDates.checkin, checkout = randomDates.checkout },
                additionalneeds = null
            }; // test "seed" Booking

            string contentBody = JsonSerializer.Serialize(booking);

            // Act
            HttpResponseMessage response = await httpClient.PostAsync("booking", new StringContent(contentBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            CreatedBooking = JsonSerializer.Deserialize<BookingRoot>(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(CreatedBooking);
            Assert.IsTrue(CreatedBooking.bookingid > 0);
            Assert.AreEqual(CreatedBooking.booking.firstname, "Aleksey");
            Assert.AreEqual(CreatedBooking.booking.lastname, "Kulikov");
            Assert.IsTrue(CreatedBooking.booking.totalprice >= 50 && CreatedBooking.booking.totalprice < 1000);
            Assert.IsNotEmpty(CreatedBooking.booking.bookingdates.checkin);
            Assert.IsNotEmpty(CreatedBooking.booking.bookingdates.checkout);
            Assert.IsNull(CreatedBooking.booking.additionalneeds);
        }

        [Test, Order(2)]
        public async Task CreatedBooking_ShouldBe_Updated()
        {
            // Arrange
            var newCheckoutDate = DateTime.Parse(CreatedBooking.booking.bookingdates.checkout).AddDays(2);
            Booking newBooking = new()
            {
                firstname = "Alexei",
                lastname = "Koulikov",
                totalprice = CreatedBooking.booking.totalprice + 50,
                depositpaid = true,
                bookingdates = new BookingDates() { checkin = CreatedBooking.booking.bookingdates.checkin, checkout = newCheckoutDate.ToString("yyyy-MM-dd") },
                additionalneeds = "Air conditioning"
            }; // fully updated Booking

            string contentBody = JsonSerializer.Serialize(newBooking);

            // Act
            HttpResponseMessage response = await httpClient.PutAsync($"booking/{CreatedBooking.bookingid}", new StringContent(contentBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var updatedBooking = JsonSerializer.Deserialize<Booking>(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(updatedBooking);
            Assert.AreEqual(updatedBooking.firstname, "Alexei");
            Assert.AreEqual(updatedBooking.lastname, "Koulikov");
            Assert.IsTrue(updatedBooking.totalprice == CreatedBooking.booking.totalprice + 50);
            Assert.IsNotEmpty(updatedBooking.bookingdates.checkin);
            Assert.IsNotEmpty(updatedBooking.bookingdates.checkout);
            Assert.IsTrue(DateTime.Parse(updatedBooking.bookingdates.checkout) == DateTime.Parse(CreatedBooking.booking.bookingdates.checkout).AddDays(2));
            Assert.IsFalse(String.IsNullOrEmpty(updatedBooking.additionalneeds));
        }

        [Test, Order(3)]
        public async Task CreatedBooking_ShouldBe_PartiallyUpdated()
        {
            // Arrange
            var newCheckoutDate = DateTime.Parse(CreatedBooking.booking.bookingdates.checkout).AddDays(3);
            string contentBody = JsonSerializer.Serialize(new 
            { 
                firstname = "Alexey", 
                totalprice = CreatedBooking.booking.totalprice + 70,
                bookingdates = new { checkout = newCheckoutDate.ToString("yyyy-MM-dd") }
            }); // partial update data

            // Act
            HttpResponseMessage response = await httpClient.PatchAsync($"booking/{CreatedBooking.bookingid}", new StringContent(contentBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var updatedBooking = JsonSerializer.Deserialize<Booking>(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(updatedBooking);
            Assert.AreEqual(updatedBooking.firstname, "Alexey");
            Assert.AreEqual(updatedBooking.lastname, "Koulikov");
            Assert.IsTrue(updatedBooking.totalprice == CreatedBooking.booking.totalprice + 70);
            Assert.IsNotEmpty(updatedBooking.bookingdates.checkin);
            Assert.IsNotEmpty(updatedBooking.bookingdates.checkout);
            Assert.IsTrue(DateTime.Parse(updatedBooking.bookingdates.checkout) == DateTime.Parse(CreatedBooking.booking.bookingdates.checkout).AddDays(3));
            Assert.IsFalse(String.IsNullOrEmpty(updatedBooking.additionalneeds));
        }

        [Test, Order(4)]
        public async Task Ensure_That_BookingId_Exists()
        {
            // Arrange
            List<BookingRoot> bookingIds = new();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync("booking");
            var responseBody = await response.Content.ReadAsStringAsync();
            bookingIds = JsonSerializer.Deserialize<List<BookingRoot>>(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(bookingIds.Any(x => x.bookingid == CreatedBooking.bookingid));
        }

        [Test, Order(5)]
        public async Task CreatedBooking_ShouldBe_Deleted()
        {
            // Arrange

            // Act
            HttpResponseMessage response = await httpClient.DeleteAsync($"booking/{CreatedBooking.bookingid}");

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test, Order(6)]
        public async Task Ensure_That_BookingId_DoesntExist()
        {
            // Arrange
            List<BookingRoot> bookingIds = new();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync("booking");
            var responseBody = await response.Content.ReadAsStringAsync();
            bookingIds = JsonSerializer.Deserialize<List<BookingRoot>>(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsFalse(bookingIds.Any(x => x.bookingid == CreatedBooking.bookingid));
        }
    }
}
