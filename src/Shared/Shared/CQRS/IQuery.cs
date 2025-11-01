using MediatR;

namespace Shared.CQRS
{
    /// <summary>
    /// Represents a MediatR query that returns a result of type <typeparamref name="T"/>.
    /// This is a marker interface built on top of <see cref="IRequest{T}"/> to express
    /// intent for read/query operations within the application's CQRS pattern.
    /// </summary>
    /// <typeparam name="T">The non-nullable result type returned by the query.</typeparam>
    public interface IQuery<out T> : IRequest<T> where T : notnull
    {
    }
}
