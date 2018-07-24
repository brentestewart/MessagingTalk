using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertManager.Dispachers;
using ConcertManager.Messages;
using ConcertManager.Repositories;

namespace ConcertManager.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderRepo = new OrderRepository();
            StorageManager.OrderRepository = orderRepo;
            TicketManager.OrderRepository = orderRepo;
            TicketManager.TotalTicketsAvailable = 500;
            TicketManager.RemainingTickets = 500;

            AppEntryPoint.TakeOrder(4, 50M, "1234-1234-1234-1223");

            System.Console.ReadKey();
        }
    }
}
