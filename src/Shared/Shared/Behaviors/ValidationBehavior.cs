using FluentValidation;
using MediatR;
using Shared.Contracts.CQRS;

namespace Shared.Behaviors
{

    // If we want to use this behavior, TRequest type must implement the interface ICommand<TResponse>.
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {

            _validators = validators;
        }

        /// <summary>
        /// Executes validation for the incoming request before invoking the next pipeline delegate/handler.
        /// </summary>
        /// <param name="request">The incoming command/request to validate.</param>
        /// <param name="next">The next delegate in the MediatR pipeline (usually the handler).</param>
        /// <param name="cancellationToken">Cancellation token propagated to asynchronous operations.</param>
        /// <returns>The response produced by the next delegate if validation succeeds.</returns>
        /// <exception cref="ValidationException">Thrown when one or more validators report validation failures.</exception>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators == null || !_validators.Any())
            {
                return await next(cancellationToken);
            }

            var context = new ValidationContext<TRequest>(request);

            // Execute all validators in parallel and collect their results.
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Flatten all errors from all validation results into a single list.
            var failures = validationResults
                .Where(r => r.Errors != null && r.Errors.Count > 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }

            // No validation errors; continue to the next behavior/handler in the pipeline.
            return await next(cancellationToken);
        }
    }
}
