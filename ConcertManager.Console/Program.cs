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
            var innerDispatcher = new MessageDispatcher();
            var dispatcher = new LoggingMessageDispatcher(innerDispatcher);

            var entryPoint = new AppEntryPoint(dispatcher);
            var orderProcessor = new OrderProcessor(dispatcher);
            var ticketManager = new TicketManager(dispatcher, orderRepo, 500);
            var feeManager = new BasicFeeManager(dispatcher);
            var taxManager = new TaxManager(dispatcher);
            var paymentManager = new PaymentManager(dispatcher);
            var storageManager = new StorageManager(dispatcher, orderRepo);

            dispatcher.Subscribe<OrderPlaced>(orderProcessor);
            dispatcher.Subscribe<TicketsReserved>(orderProcessor);
            dispatcher.Subscribe<FeesCalculated>(orderProcessor);
            dispatcher.Subscribe<TaxesCalculated>(orderProcessor);
            dispatcher.Subscribe<CreditCardCharged>(orderProcessor);
            dispatcher.Subscribe(ticketManager);
            dispatcher.Subscribe(feeManager);
            dispatcher.Subscribe(taxManager);
            dispatcher.Subscribe(paymentManager);
            dispatcher.Subscribe(storageManager);

            entryPoint.TakeOrder(4, 50M, "1234-1234-1234-1223");

            System.Console.ReadKey();
        }
    }
}
