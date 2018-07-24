using System;
using System.Collections.Generic;
using System.Linq;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class AppEntryPoint
    {
        public static void TakeOrder(int ticketCount, decimal ticketPrice, string creditCardNumber)
        {
            var order = new Order
            {
                CreditCardNumber = creditCardNumber,
                TicketCount = ticketCount,
                Subtotal = ticketCount * ticketPrice
            };

            OrderProcessor.ProcessOrder(order);

            Console.WriteLine($"Order {order.OrderId} is complete.");
        }
    }
}