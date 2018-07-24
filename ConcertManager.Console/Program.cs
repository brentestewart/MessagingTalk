using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertManager.Dispachers;
using ConcertManager.Messages;
using ConcertManager.Repositories;
using NServiceBus;
using NServiceBus.Logging;

namespace ConcertManager.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            System.Console.Title = "ConcertManager Main";
            var orderRepo = new OrderRepository();
            var innerDispatcher = new MessageDispatcher();
            var dispatcher = new LoggingMessageDispatcher(innerDispatcher);

            var paymentEndpointConfiguration = new EndpointConfiguration("ClientUI");
            paymentEndpointConfiguration.RegisterComponents(configureComponents =>
            {
                configureComponents.RegisterSingleton(typeof(IPublisher), dispatcher);
            });

            var transport = paymentEndpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(ChargeCreditCard), "Payments");

            var paymentEndpointInstance = await Endpoint.Start(paymentEndpointConfiguration)
                .ConfigureAwait(false);

            var entryPoint = new AppEntryPoint(dispatcher);
            var orderProcessor = new OrderProcessor(dispatcher);
            var ticketManager = new TicketManager(dispatcher, orderRepo, 500);
            var feeManager = new BasicFeeManager(dispatcher);
            var taxManager = new TaxManager(dispatcher);
            var paymentManager = new PaymentManager(dispatcher, paymentEndpointInstance);
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
            dispatcher.Subscribe(entryPoint);

            await Task.Delay(5000);
            entryPoint.TakeOrder(4, 50M, "1234-1234-1234-1223");

            System.Console.ReadKey();

            await paymentEndpointInstance.Stop()
                .ConfigureAwait(false);
        }

        static ILog log = LogManager.GetLogger<Program>();
    }
}
