using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class BasicFeeManager : IHandle<CalculateFees>
    {
        public IPublisher Publisher { get; }

        public BasicFeeManager(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void Handle(CalculateFees command)
        {
            var order = command.Order;
            order.Fees = order.Subtotal * .10M;

            Publisher.Publish(new FeesCalculated() { Order =  order});
        }
    }
}