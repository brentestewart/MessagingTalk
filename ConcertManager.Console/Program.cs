using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
            //var innerDispatcher = new MessageDispatcher();
            //var dispatcher = new LoggingMessageDispatcher(innerDispatcher);
            var dispatcher = new MessageDispatcher();

            var ordersCompeted = new ConcurrentBag<Guid>();
            var entryPoint = new AppEntryPoint(dispatcher, ordersCompeted);
            var orderProcessor = new OrderProcessor(dispatcher);
            var ticketManager = new TicketManager(dispatcher, orderRepo, 500);
            var feeManager = new BasicFeeManager(dispatcher);
            var taxManager = new TaxManager(dispatcher);

            //var paymentManager = new PaymentManager(dispatcher);
            var paymentsLog = new ConcurrentDictionary<Guid, string>();
            var paymentManagers = new List<PaymentManager>
            {
                new PaymentManager("Manager 1", dispatcher, paymentsLog, .1m),
                new PaymentManager("Manager 2", dispatcher, paymentsLog, .5m),
                new PaymentManager("Manager 3", dispatcher, paymentsLog, 1m)
            };
            var threadedPaymentManagers = paymentManagers.Select(p => new ThreadedHandler<ChargeCreditCard>(p)).ToList();
            var paymentProcessor = new ThreadedHandler<ChargeCreditCard>(new RoundRobinDispatcher<ChargeCreditCard>(threadedPaymentManagers));

            var storageManager = new StorageManager(dispatcher, orderRepo);

            dispatcher.Subscribe<OrderPlaced>(orderProcessor);
            dispatcher.Subscribe<TicketsReserved>(orderProcessor);
            dispatcher.Subscribe<FeesCalculated>(orderProcessor);
            dispatcher.Subscribe<TaxesCalculated>(orderProcessor);
            dispatcher.Subscribe<CreditCardCharged>(orderProcessor);

            dispatcher.Subscribe(ticketManager);
            dispatcher.Subscribe(feeManager);
            dispatcher.Subscribe(taxManager);
            dispatcher.Subscribe(paymentProcessor);
            dispatcher.Subscribe(storageManager);
            dispatcher.Subscribe(entryPoint);

            paymentProcessor.Start();
            threadedPaymentManagers.ForEach(m => m.Start());

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var numberOfOrders = 51;
            for (int i = 0; i < numberOfOrders; i++)
            {
                entryPoint.TakeOrder(4, 50M, "1234-1234-1234-1223");
            }

            while (ordersCompeted.Count < numberOfOrders)
            {
                Thread.Sleep(1);
            }
            stopwatch.Stop();

            var allPayments = paymentsLog.ToList();
            var manager1OrderCount = allPayments.Count(p => p.Value == "Manager 1");
            var manager2OrderCount = allPayments.Count(p => p.Value == "Manager 2");
            var manager3OrderCount = allPayments.Count(p => p.Value == "Manager 3");

            System.Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}ms");
            System.Console.WriteLine($"Manager 1 processed {manager1OrderCount} orders.");
            System.Console.WriteLine($"Manager 2 processed {manager2OrderCount} orders.");
            System.Console.WriteLine($"Manager 3 processed {manager3OrderCount} orders.");
            System.Console.ReadKey();
        }
    }
}
