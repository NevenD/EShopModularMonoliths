namespace Shared.DDD
{
    public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
    {
        // Internal list to store domain events raised by the aggregate.
        private readonly List<IDomainEvent> _domainEvents = [];

        // Exposes domain events as a read-only list for external consumers.
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // Adds a domain event to the aggregate's event list.
        public void AdDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        // Clears all domain events from the aggregate and returns them as an array.
        public IDomainEvent[] ClearDomainEvents()
        {
            IDomainEvent[] dequedEvents = _domainEvents.ToArray();
            _domainEvents.Clear();

            return dequedEvents;
        }
    }
}
