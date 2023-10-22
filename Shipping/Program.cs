using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Shpping
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Shipping";

            await Host.CreateDefaultBuilder(args).UseNServiceBus(context =>
            {
                var endPointConfiguration = new EndpointConfiguration("Shipping");

                //endPointConfiguration.UseTransport<LearningTransport>();
                var transport = endPointConfiguration.UseTransport<RabbitMQTransport>();
                transport.ConnectionString("host=localhost;username=guest;password=guest");
                transport.UseConventionalRoutingTopology(QueueType.Classic);

                endPointConfiguration.UsePersistence<LearningPersistence>();

                endPointConfiguration.EnableInstallers();
                
                return endPointConfiguration;

            }).RunConsoleAsync();
        }
    }
}