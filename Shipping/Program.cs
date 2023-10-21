using Microsoft.Extensions.Hosting;

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
                endPointConfiguration.UseTransport<LearningTransport>();
                endPointConfiguration.UsePersistence<LearningPersistence>();
                return endPointConfiguration;
            }).RunConsoleAsync();
        }
    }
}