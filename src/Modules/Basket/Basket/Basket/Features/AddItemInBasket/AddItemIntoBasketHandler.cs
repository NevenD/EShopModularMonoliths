using Basket.Basket.Dtos;
using Basket.Data.Repository;
using Shared.CQRS;

namespace Basket.Basket.Features.AddItemInBasket
{
    public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem) : ICommand<AddItemInBasketResult>;
    public record AddItemInBasketResult(Guid Id);

    public sealed class AddItemIntoBasketHandler : ICommandHandler<AddItemIntoBasketCommand, AddItemInBasketResult>
    {
        private readonly IBasketRepository _basketRepository;

        public AddItemIntoBasketHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<AddItemInBasketResult> Handle(AddItemIntoBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasket(request.UserName, false, cancellationToken);

            shoppingCart.AddItem(
                request.ShoppingCartItem.ProductId,
                request.ShoppingCartItem.Quantity,
                request.ShoppingCartItem.Color,
                request.ShoppingCartItem.Price,
                request.ShoppingCartItem.ProductName);

            await _basketRepository.SaveChangesAsync(cancellationToken);

            return new AddItemInBasketResult(shoppingCart.Id);
        }
    }
}
