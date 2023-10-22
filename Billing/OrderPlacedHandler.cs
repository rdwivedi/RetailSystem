using Messages;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing
{
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        async Task IHandleMessages<OrderPlaced>.Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            // intentionally delaying the order billed process so that Shipping Saga can timeout
            await Task.Delay(TimeSpan.FromSeconds(10));

            log.Info($"Received OrderPlace, OrderId = {message.OrderId}");

            await context.Publish(new OrderBilled { OrderId = message.OrderId });
        }
    }
}
