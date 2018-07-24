using System;
using System.Text;
using LiteDB;

namespace ConcertManager.Repositories
{
    public class OrderRepository
    {
        public LiteDatabase Database { get; set; }
        public OrderRepository()
        {
            Database = new LiteDatabase("Orders.db");
        }
        public void StoreOrder(Order order)
        {
            var orders = Database.GetCollection<Order>("orders");
            orders.Insert(order);
        }

        public int GetSoldTicketCount()
        {
            var orders = Database.GetCollection<Order>("orders");
            var allOrders = orders.Find(Query.All());
            var total = 0;
            foreach (var order in allOrders)
            {
                total += order.TicketCount;
            }

            return total;
        }
    }
}
