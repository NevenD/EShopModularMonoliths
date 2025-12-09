using Basket.Data.Repository;
using Shared.Contracts.CQRS;

namespace Basket.Basket.Features.RemoveItemFromBasket
{
    public record RemoveItemFromBasketCommand(string UserName, Guid ProductId) : ICommand<RemoveItemFromBasketResult>;

    public record RemoveItemFromBasketResult(Guid Id);

    public sealed class RemoveItemFromBasketHandler : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
    {
        private readonly IBasketRepository _basketRepository;

        public RemoveItemFromBasketHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasket(request.UserName, false, cancellationToken);

            shoppingCart.RemoveItem(request.ProductId);
            await _basketRepository.SaveChangesAsync(request.UserName, cancellationToken);

            return new RemoveItemFromBasketResult(shoppingCart.Id);
        }
    }
}
