using Messages;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales
{
    internal class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();

        static Random rnd = new Random();

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            // intentionalyy making messaged to fail.
            //if (rnd.Next(0, 100) > 10)
            //{
            //    throw new Exception("BOOM");
            //}

            log.Info($"Received PlaceOrder, OrderID = {message.OrderID}");

            await context.Publish(new OrderPlaced { OrderId = message.OrderID});
        }
    }
}
