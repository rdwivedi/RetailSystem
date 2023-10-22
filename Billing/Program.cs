using Microsoft.Extensions.Hosting;

namespace Billing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Billing";

            await Host.CreateDefaultBuilder(args).UseNServiceBus(context =>
            {
                var endPointConfiguration = new EndpointConfiguration("Billing"); 


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