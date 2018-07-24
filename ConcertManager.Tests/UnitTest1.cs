using System;
using ConcertManager.Dispachers;
using ConcertManager.Messages;
using ConcertManager.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConcertManager.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ValidateFeeCalculation()
        {
            var dispatcher = new Dispachers.MessageDispatcher();
            var order = new Order() { TicketCount = 4, Subtotal = 50.0M};
            var message = new CalculateFees() {Order = order};

            var feeManager = new BasicFeeManager(dispatcher);
            dispatcher.Subscribe<CalculateFees>(feeManager);

            dispatcher.Publish(message);

            Assert.AreEqual(5.0M, order.Fees);
        }

        [TestMethod]
        public void TestEverything()
        {
            var innerDispatcher = new MessageDispatcher();
            var dispatcher = new LoggingMessageDispatcher(innerDispatcher);
            var orderRepo = new OrderRepository();

            var entryPoint = new AppEntryPoint(dispatcher);
            var order = new Order { TicketCount = 4, Subtotal = 100M };
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
        }
    }
}
