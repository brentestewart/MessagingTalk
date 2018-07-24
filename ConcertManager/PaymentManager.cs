using System;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class PaymentManager
    {
        private static Random RandomGenerator { get; set; } = new Random();

        public static string ChargeCrditCard(Order order)
        {
            var confirmation = RandomGenerator.Next(10000, 99999).ToString();
            Console.WriteLine($"Order: {order.OrderId} -- Charged card with confirmation code of {confirmation}");
            return confirmation;
        }
    }
}