using System;
using System.Collections.Concurrent;
using System.Threading;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class PaymentManager : IHandle<ChargeCreditCard>
    {
        private static Random RandomGenerator { get; set; } = new Random();
        public string Name { get; }
        public IPublisher Publisher { get; }
        public ConcurrentDictionary<Guid, string> PaymentsLog { get; }
        public decimal SpeedFactor { get; }

        public PaymentManager(string name, IPublisher publisher, ConcurrentDictionary<Guid, string> paymentsLog, decimal speedFactor)
        {
            Name = name;
            Publisher = publisher;
            PaymentsLog = paymentsLog;
            SpeedFactor = speedFactor;
        }

        public void Handle(ChargeCreditCard t)
        {
            var order = t.Order;
            
            order.PaymentConfirmation = RandomGenerator.Next(10000, 99999).ToString();

            var processingTime = (int)(RandomGenerator.Next(1, 50) * SpeedFactor);
            Thread.Sleep(processingTime);
            var worked = PaymentsLog.TryAdd(order.OrderId, Name);
            //Console.WriteLine($"Credit Card Processed by {Name} : {processingTime}");
            Publisher.Publish(new CreditCardCharged { Order = order });
        }
    }
}