using System;
using System.Collections.Generic;
using ConcertManager.Dispachers;
using ConcertManager.Messages;
using ConcertManager.Repositories;
using NServiceBus;

namespace ConcertManager
{
    public class TicketManager : IHandle<ReserveTickets>
    {
        public IPublisher Publisher { get; }
        public OrderRepository OrderRepository { get; }
        public int TotalTicketsAvailable { get; }
        public int RemainingTickets { get; private set; }

        public TicketManager(IPublisher publisher, OrderRepository orderRepository, int availableTickets)
        {
            Publisher = publisher;
            OrderRepository = orderRepository;
            TotalTicketsAvailable = availableTickets;
            RemainingTickets = TotalTicketsAvailable - OrderRepository.GetSoldTicketCount();
        }

        public void Handle(ReserveTickets t)
        {
            var order = t.Order;

            RemainingTickets -= order.TicketCount;

            Publisher.Publish(new TicketsReserved() { Order = order });
        }
    }
}
