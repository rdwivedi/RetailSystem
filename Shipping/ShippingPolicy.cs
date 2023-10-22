using Messages;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shipping.ShippingPolicy;

namespace Shipping
{
    internal class ShippingPolicy : Saga<ShippingPolicy.SagaData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>,
        IHandleTimeouts<ShippingPolicy.OrderNotBilledTimeOut>
    {
        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            this.Data.Placed = true;
            log.Info($"Received OrderPlaced, OrderID = {message.OrderId}, should we ship? Placed = {Data.Placed}, Billed = {Data.Billed}");

            await ProcessOrder(context);

            //saga timeout - assuming that Billed does not happen within 5 seconds
            if (!Data.Billed)
            {
                await this.RequestTimeout<OrderNotBilledTimeOut>(context, TimeSpan.FromSeconds(5));
            }
        }



        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            this.Data.Billed = true;
            log.Info($"Received OrderBilled, OrderID = {message.OrderId}, should we ship? Placed = {Data.Placed}, Billed = {Data.Billed}");

            await ProcessOrder(context);
        }

        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.Placed && Data.Billed)
            {
                log.Info("Orderl Placed, Order Billed...Time to Ship!!");
                MarkAsComplete();
            }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.MapSaga(sagaData => sagaData.OrderId).
                 ToMessage<OrderPlaced>(msg => msg.OrderId).
                 ToMessage<OrderBilled>(msg => msg.OrderId);

        }

        public Task Timeout(OrderNotBilledTimeOut state, IMessageHandlerContext context)
        {
            log.Info($" Ou-oh Order {Data.OrderId} has not been builled, publishing OrderNotBilled...");
            return Task.CompletedTask;
        }

        public class SagaData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool Billed { get; set; }
            public bool Placed { get; set; }
        }

        public class OrderNotBilledTimeOut
        {
        }
    }
}
