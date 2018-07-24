using System;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class PaymentManager : IHandle<ChargeCreditCard>
    {
        private Random RandomGenerator { get; set; } = new Random();
        public IPublisher Publisher { get; }

        public PaymentManager(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void Handle(ChargeCreditCard t)
        {
            var order = t.Order;
            
            // TODO:Charge card
            order.PaymentConfirmation = RandomGenerator.Next(10000, 99999).ToString();

            Publisher.Publish(new CreditCardCharged { Order = order });
        }
    }
}