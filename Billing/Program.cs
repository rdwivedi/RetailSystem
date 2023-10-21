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
                endPointConfiguration.UseTransport<LearningTransport>();
                return endPointConfiguration;
            }).RunConsoleAsync();
        }
    }
}