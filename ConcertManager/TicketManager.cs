using System;
using System.Collections.Generic;
using ConcertManager.Dispachers;
using ConcertManager.Messages;
using ConcertManager.Repositories;

namespace ConcertManager
{
    public class TicketManager
    {
        public static OrderRepository OrderRepository { get; set; }
        public static int TotalTicketsAvailable { get; set; }
        public static int RemainingTickets { get; set; }

        public static void ReserveTickets(Order order)
        {
            RemainingTickets -= order.TicketCount;
            Console.WriteLine($"Order: {order.OrderId} -- Reserved {order.TicketCount} tickets for {order.Subtotal:C}.");
        }
    }
}
