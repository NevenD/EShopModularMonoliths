using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Products.EventHandler
{
    public sealed class ProductPriceChangeEventHandler : INotificationHandler<ProductPriceChangedEvent>
    {
        private readonly ILogger<ProductCreatedEventHandler> _logger;

        public ProductPriceChangeEventHandler(ILogger<ProductCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
