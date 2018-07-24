using ConcertManager.Dispachers;
using ConcertManager.Messages;
using ConcertManager.Repositories;

namespace ConcertManager
{
    public class StorageManager
    {
        public static OrderRepository OrderRepository { get; set; }

        public void StoreOrder(Order order)
        {
            OrderRepository.StoreOrder(order);
        }
    }
}