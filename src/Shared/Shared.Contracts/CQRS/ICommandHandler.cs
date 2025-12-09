using MediatR;

namespace Shared.Contracts.CQRS
{




    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
        where TCommand : ICommand<Unit>
    { }

    /// <summary>
    /// Represents a handler for a command in the CQRS pattern.
    /// A command is an operation that changes state and returns a response.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of command this handler can process.
    /// Contravariant (in) allows a handler of a more derived command type
    /// to be assigned to a variable expecting a base command type.
    /// </typeparam>
    /// <typeparam name="TResponse">
    /// The type of response returned by handling the command.
    /// Must be non-nullable.
    /// </typeparam>
    public interface ICommandHandler<in TCommand, TResponse>
        : IRequestHandler<TCommand, TResponse> // Inherits MediatR's IRequestHandler for handling requests
        where TCommand : ICommand<TResponse>   // Ensures the command implements ICommand<TResponse>
        where TResponse : notnull              // Ensures the response cannot be null
    {
        // Marker interface: no additional members.
        // Used to distinguish command handlers from query handlers in CQRS.
    }
}
