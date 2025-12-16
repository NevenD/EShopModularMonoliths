using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandler
{
    public sealed class ProductPriceChangeEventHandler : INotificationHandler<ProductPriceChangedEvent>
    {
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IBus _bus;

        public ProductPriceChangeEventHandler(ILogger<ProductCreatedEventHandler> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);

            var integrationEvent = new ProductPriceChangedIntegrationEvent
            {
                ProductId = notification.Product.Id,
                Name = notification.Product.Name,
                Category = notification.Product.Category,
                Description = notification.Product.Description,
                ImageFile = notification.Product.ImageFile,
                Price = notification.Product.Price,
            };

            await _bus.Publish(integrationEvent, cancellationToken);

        }
    }
}
