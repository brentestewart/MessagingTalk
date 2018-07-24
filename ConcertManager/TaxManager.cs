using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class TaxManager : IHandle<CalculateTaxes>
    {
        public IPublisher Publisher { get; }
        private readonly decimal TaxRate = .10M;

        public TaxManager(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void Handle(CalculateTaxes t)
        {
            var order = t.Order;

            order.Taxes = (order.Subtotal + order.Fees) * TaxRate;

            Publisher.Publish(new TaxesCalculated { Order = order });
        }
    }
}