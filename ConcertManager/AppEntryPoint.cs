using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class AppEntryPoint : IHandle<OrderCommited>
    {
        public List<Order> PendingOrders { get; set; } = new List<Order>();
        public IPublisher Publisher { get; }
        private ConcurrentBag<Guid> OrdersComplete { get; set; }

        public AppEntryPoint(IPublisher publisher, ConcurrentBag<Guid> ordersComplete)
        {
            Publisher = publisher;
            OrdersComplete = ordersComplete;
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

            OrdersComplete.Add(pendingOrder.OrderId);
            //Console.WriteLine($"Order Complete: {pendingOrder.OrderId}");
        }
    }
}