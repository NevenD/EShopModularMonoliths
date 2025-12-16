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
        public Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
        {
            _logger.LogInformation("Integration Event handled: {IntegrationEvent}", context);

            // mediatr new command and handler to find products 

            return Task.CompletedTask;
        }
    }
}
