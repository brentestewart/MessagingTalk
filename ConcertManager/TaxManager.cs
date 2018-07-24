using System;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class TaxManager
    {
        private static readonly decimal TaxRate = .10M;

        public static decimal CalculateTaxes(Order order)
        {
            var taxes = (order.Subtotal + order.Fees) * TaxRate;
            Console.WriteLine($"Order: {order.OrderId} -- Calculated taxes of {taxes:C}");
            return taxes;
        }
    }
}