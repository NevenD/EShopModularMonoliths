using Basket.Basket.Features.UpdatePriceItemInBasket;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers
{
    public class ProductPriceChangedIntegrationEventHandler : IConsumer<ProductPriceChangedIntegrationEvent>
    {
        private readonly ILogger<ProductPriceChangedIntegrationEventHandler> _logger;
        private readonly ISender _sender;
        public ProductPriceChangedIntegrationEventHandler(ILogger<ProductPriceChangedIntegrationEventHandler> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        // this method is invoked when product price changed is recieved from mass transit
        public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
        {
            _logger.LogInformation("Integration Event handled: {IntegrationEvent}", context);

            var command = new UpdateItemPriceInBasketCommand(context.Message.ProductId, context.Message.Price);
            var result = await _sender.Send(command);

            if (result.IsSuccess)
            {
                _logger.LogError("Error updating the price: {ProductId}", context.Message.ProductId);
            }

            _logger.LogInformation("Price for product id: {ProductId} update in basket", context.Message.ProductId);
        }
    }
}
