namespace Shared.Messaging.Events
{
    public record IntegrationEvent
    {
        public static Guid EventId => Guid.NewGuid();
        public static DateTime OccuredOn => DateTime.UtcNow;
        public string EventType => GetType()?.AssemblyQualifiedName ?? string.Empty;
    }
}
