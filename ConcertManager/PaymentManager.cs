using System;
using System.Threading.Tasks;
using ConcertManager.Dispachers;
using ConcertManager.Messages;
using NServiceBus;

namespace ConcertManager
{
    public class PaymentManager : IHandle<ChargeCreditCard>
    {
        private Random RandomGenerator { get; set; } = new Random();
        public IPublisher Publisher { get; }
        public IEndpointInstance PaymentEndpoint { get; set; }

        public PaymentManager(IPublisher publisher, IEndpointInstance paymentEndpoint)
        {
            Publisher = publisher;
            PaymentEndpoint = paymentEndpoint;
        }

        public void Handle(ChargeCreditCard t)
        {
            var order = t.Order;
            
            PaymentEndpoint.Send(t);
        }
    }

    public class PaymentResponseManager : IHandleMessages<CreditCardCharged>
    {
        public IPublisher Publisher { get; }

        public PaymentResponseManager(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public Task Handle(CreditCardCharged message, IMessageHandlerContext context)
        {
            Publisher.Publish(message);
            return Task.CompletedTask;
        }
    }
}