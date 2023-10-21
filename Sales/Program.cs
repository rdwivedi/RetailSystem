using Microsoft.Extensions.Hosting;

namespace Sales
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Sales";

            await Host.CreateDefaultBuilder(args).UseNServiceBus(context =>
            {
                var endPointConfiguration = new EndpointConfiguration("Sales");
                endPointConfiguration.UseTransport<LearningTransport>();
                return endPointConfiguration;
            }).RunConsoleAsync();
        }
    }
}