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

            //System.Console.ReadKey();
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
            //await RunLoop(endpointInstance)
                //.ConfigureAwait(false);

            await paymentEndpointInstance.Stop()
                .ConfigureAwait(false);
        }

        static ILog log = LogManager.GetLogger<Program>();

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to place an order, or 'Q' to quit.");
                var key = System.Console.ReadKey();
                System.Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command
                        var command = new ChargeCreditCard()
                        {
                            Order = new Order() {Subtotal = 50m}
                        };

                        // Send the command
                        log.Info($"Sending PlaceOrder command, OrderId = {command.Order.OrderId}");
                        await endpointInstance.Send(command)
                            .ConfigureAwait(false);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}
