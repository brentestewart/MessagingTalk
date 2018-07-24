using ConcertManager.Dispachers;
using ConcertManager.Messages;
using ConcertManager.Repositories;

namespace ConcertManager
{
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