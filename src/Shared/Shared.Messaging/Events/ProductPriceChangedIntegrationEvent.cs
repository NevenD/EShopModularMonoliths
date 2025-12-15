namespace Shared.Messaging.Events
{
    public record ProductPriceChangedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
        public List<string> Category { get; set; } = [];
        public string Description { get; set; } = string.Empty;
        public string ImageFile { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
