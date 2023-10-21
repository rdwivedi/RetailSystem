using Messages;
using NServiceBus;
using System.Threading.Tasks;

namespace ClientUI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "ClientUI";

            var endPointConfiguration = new EndpointConfiguration("ClientUI");

            var transport = endPointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();

            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales") ;

            endPointConfiguration.SendOnly();

            var endPoint = await Endpoint.Start(endPointConfiguration);

            await Runloop(endPoint);
        }

        private static async Task Runloop(IMessageSession endPoint)
        {
            while (true)
            {
                Console.WriteLine("P to place order, Q to exit");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.P:
                       await endPoint.Send(new PlaceOrder { OrderID = Guid.NewGuid().ToString().Substring(0, 8) });
                        break;

                    case ConsoleKey.Q:
                        return;
                }
            }


        }
    }
}