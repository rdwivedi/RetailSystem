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

                //endPointConfiguration.UseTransport<LearningTransport>();
                var transport = endPointConfiguration.UseTransport<RabbitMQTransport>();
                transport.ConnectionString("host=localhost;username=guest;password=guest");
                transport.UseConventionalRoutingTopology(QueueType.Classic);

                endPointConfiguration.UsePersistence<LearningPersistence>();

                endPointConfiguration.EnableInstallers();

                // Recoverability is the built-in error handling capability. Recoverability enables to recover automatically, or in exceptional scenarios manually, from message failures.
                //https://docs.particular.net/nservicebus/recoverability/
                RecoverabilitySettings recoverabilitySettings = endPointConfiguration.Recoverability();
                recoverabilitySettings.Immediate(settings => settings.NumberOfRetries(0));
                recoverabilitySettings.Delayed(settings => settings.NumberOfRetries(0));    

                return endPointConfiguration;

            }).RunConsoleAsync();
        }
    }
}