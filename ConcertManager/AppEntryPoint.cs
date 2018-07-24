using System;
using System.Collections.Generic;
using System.Linq;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class AppEntryPoint : IHandle<OrderCommited>
    {
        public List<Order> PendingOrders { get; set; } = new List<Order>();
        public IPublisher Publisher { get; }

        public AppEntryPoint(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void TakeOrder(int ticketCount, decimal ticketPrice, string creditCardNumber)
        {
            var order = new Order
            {
                CreditCardNumber = creditCardNumber,
                TicketCount = ticketCount,
                Subtotal = ticketCount * ticketPrice
            };

            PendingOrders.Add(order);

            Publisher.Publish(new OrderPlaced { Order = order });
        }

        public void Handle(OrderCommited t)
        {
            var pendingOrder = PendingOrders.FirstOrDefault(o => o.OrderId == t.Order.OrderId);

            if(pendingOrder == null) return;

            PendingOrders.Remove(pendingOrder);

            Console.WriteLine($"Order Complete: {pendingOrder.OrderId}");
        }
    }
}