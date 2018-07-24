using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertManager.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace ConcertManager.PaymentProcessor
{
    public class ChargeCreditCardHandler : IHandleMessages<ChargeCreditCard>
    {
        private Random RandomGenerator { get; set; } = new Random();
        static ILog log = LogManager.GetLogger<ChargeCreditCardHandler>();

        public Task Handle(ChargeCreditCard message, IMessageHandlerContext context)
        {
            var order = message.Order;
            log.Info($"Received PlaceOrder, OrderId = {order.OrderId}");
            order.PaymentConfirmation = RandomGenerator.Next(10000, 99999).ToString();

            // Fire Processed Credit Card Event
            var response = new CreditCardCharged() {Order = order};
            return context.Publish(response);
        }
    }
}
