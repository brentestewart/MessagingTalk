using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class OrderProcessor : IOrderProcessor
    {
        public IPublisher Publisher { get; }

        public OrderProcessor(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void Handle(OrderPlaced t)
        {
            Publisher.Publish(new ReserveTickets { Order = t.Order });
        }

        public void Handle(TicketsReserved t)
        {
            Publisher.Publish(new CalculateFees { Order = t.Order });
        }

        public void Handle(FeesCalculated t)
        {
            Publisher.Publish(new CalculateTaxes { Order = t.Order });    
        }
        
        public void Handle(TaxesCalculated t)
        {
            Publisher.Publish(new ChargeCreditCard { Order = t.Order });
        }

        public void Handle(CreditCardCharged t)
        {
            Publisher.Publish(new CommitOrder { Order = t.Order });
        }
    }
}