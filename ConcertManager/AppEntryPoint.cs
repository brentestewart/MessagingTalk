using System;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class AppEntryPoint
    {
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

            Publisher.Publish(new OrderPlaced { Order = order });
        }

    }
}