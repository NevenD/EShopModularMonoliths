using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptors
{
    /// <summary>
    /// Intercepts SaveChanges and SaveChangesAsync calls on a <see cref="DbContext"/>
    /// to dispatch domain events found on aggregates implementing <see cref="IAggregate"/>.
    /// </summary>
    /// <remarks>
    /// This interceptor finds domain events collected on aggregates via the DbContext.ChangeTracker,
    /// clears them from the aggregates, and publishes each event through the provided <see cref="IMediator"/>.
    /// The synchronous <see cref="SavingChanges"/> override blocks on the asynchronous dispatch operation.
    /// </remarks>
    /// <param name="mediator">Mediator used to publish domain events.</param>
    public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        /// <summary>
        /// Called before changes are saved to the database (synchronous path).
        /// This implementation dispatches domain events from tracked aggregates
        /// before the actual saving occurs.
        /// </summary>
        /// <param name="eventData">Contextual data for the save operation.</param>
        /// <param name="result">The current interception result.</param>
        /// <returns>The (possibly modified) interception result.</returns>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // Block on the async dispatch to ensure events are published before changes commit.
            DispatchDomainEventsEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        /// <summary>
        /// Called before changes are saved to the database (asynchronous path).
        /// This implementation dispatches domain events from tracked aggregates
        /// before the actual saving occurs.
        /// </summary>
        /// <param name="eventData">Contextual data for the save operation.</param>
        /// <param name="result">The current interception result.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="ValueTask{InterceptionResult}"/> representing the interception result.</returns>
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            // Dispatch domain events asynchronously and then continue with the save pipeline.
            await DispatchDomainEventsEvents(eventData.Context).ConfigureAwait(false);
            return await base.SavingChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gathers domain events from tracked aggregates, clears them on the aggregates,
        /// and publishes each event through the <see cref="IMediator"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> whose ChangeTracker will be inspected.
        /// If null, the method returns immediately.</param>
        /// <returns>A task that completes after all domain events have been published.</returns>
        private async Task DispatchDomainEventsEvents(DbContext? context)
        {
            if (context is null)
            {
                return;
            }

            // Find aggregates with pending domain events
            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(a => a.Entity.DomainEvents.Any())
                .Select(a => a.Entity);

            // Extract all domain events and materialize to a list
            var domainEvents = aggregates
                .SelectMany(a => a.DomainEvents)
                .ToList();

            // Clear domain events from each aggregate so they won't be re-published later
            aggregates.ToList().ForEach(a => a.ClearDomainEvents());

            // Publish each domain event via mediator
            foreach (var domain in domainEvents)
            {
                await mediator.Publish(domain);
            }
        }
    }
}
