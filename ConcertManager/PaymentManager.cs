using System;
using ConcertManager.Dispachers;
using ConcertManager.Messages;
using ConcertManager.Repositories;

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

    public class StorageManager : IHandle<CommitOrder>
    {
        public IPublisher Publisher { get; }
        public OrderRepository OrderRepository { get; }

        public StorageManager(IPublisher publisher, OrderRepository orderRepository)
        {
            Publisher = publisher;
            OrderRepository = orderRepository;
        }

        public void Handle(CommitOrder t)
        {
            var order = t.Order;

            OrderRepository.StoreOrder(order);

            Publisher.Publish(new OrderCommited {Order = order});
        }
    }
}