using BookerApi.Tests.Classes;
using System;

namespace BookerApi.Tests.Helpers
{
    public static class RandomizerHelper
    {
        public static BookingDates GetRandomDates()
        {
            Random rnd = new();
            var randomCheckinMonth = rnd.Next(1, 12); // some magic numbers just to create more realistic dates
            var randomCheckinDay = rnd.Next(1, 25);
            var randomCheckoutMonth = randomCheckinMonth == 12 ? 12 : (randomCheckinMonth + 1);
            var randomCheckoutDay = randomCheckoutMonth == randomCheckinMonth ? rnd.Next(randomCheckinDay, 28) : rnd.Next(1, 28);
            var randomCheckinDate = new DateTime(2022, randomCheckinMonth, randomCheckinDay);
            var randomCheckoutDate = new DateTime(2022, randomCheckoutMonth, randomCheckoutDay);
            return new BookingDates() { checkin = randomCheckinDate.ToString("yyyy-MM-dd"), checkout = randomCheckoutDate.ToString("yyyy-MM-dd") };
        }

        public static int GetRandomPrice()
        {
            Random rnd = new();
            return rnd.Next(50, 1000);
        }
    }
}
