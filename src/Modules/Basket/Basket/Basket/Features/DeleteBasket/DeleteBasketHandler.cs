using Basket.Data.Repository;
using Shared.Contracts.CQRS;

namespace Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;

    public record DeleteBasketResult(bool IsSuccess);

    public sealed class DeleteBasketHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        private readonly IBasketRepository _basketRepository;

        public DeleteBasketHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            await _basketRepository.DeleteBasket(command.Username, cancellationToken);

            return new DeleteBasketResult(true);

        }
    }
}
