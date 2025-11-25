using Basket.Basket.Dtos;
using Basket.Basket.Exceptions;
using Basket.Data;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Basket.Features.AddItemInBasket
{
    public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem) : ICommand<AddItemInBasketResult>;
    public record AddItemInBasketResult(Guid Id);

    public sealed class AddItemIntoBasketHandler : ICommandHandler<AddItemIntoBasketCommand, AddItemInBasketResult>
    {
        private readonly BasketDbContext _basketDbContext;

        public AddItemIntoBasketHandler(BasketDbContext basketDbContext)
        {
            _basketDbContext = basketDbContext;
        }

        public async Task<AddItemInBasketResult> Handle(AddItemIntoBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketDbContext.ShoppingCarts.Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

            if (shoppingCart is null)
            {
                throw new BasketNotFoundException(request.UserName);
            }

            shoppingCart.AddItem(
                request.ShoppingCartItem.ProductId,
                request.ShoppingCartItem.Quantity,
                request.ShoppingCartItem.Color,
                request.ShoppingCartItem.Price,
                request.ShoppingCartItem.ProductName);

            await _basketDbContext.SaveChangesAsync(cancellationToken);

            return new AddItemInBasketResult(shoppingCart.Id);
        }
    }
}
