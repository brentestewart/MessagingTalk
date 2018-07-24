using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertManager.Dispachers;
using NServiceBus;

namespace ConcertManager.PaymentProcessor
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Process Payment";
            var dispatcher = new MessageDispatcher();

            var endpointConfiguration = new EndpointConfiguration("Payments");
            endpointConfiguration.RegisterComponents(configureComponents =>
            {
                configureComponents.RegisterSingleton(typeof(IPublisher), dispatcher);
            });

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
