using Basket.Basket.Exceptions;
using Basket.Data;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Basket.Features.RemoveItemFromBasket
{
    public record RemoveItemFromBasketCommand(string UserName, Guid ProductId) : ICommand<RemoveItemFromBasketResult>;

    public record RemoveItemFromBasketResult(Guid Id);

    public sealed class RemoveItemFromBasketHandler : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
    {
        private readonly BasketDbContext _basketDbContext;

        public RemoveItemFromBasketHandler(BasketDbContext basketDbContext)
        {
            _basketDbContext = basketDbContext;
        }

        public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketDbContext.ShoppingCarts
                    .AsNoTracking()
                    .Include(x => x.Items)
                    .SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);


            if (shoppingCart is null)
            {
                throw new BasketNotFoundException(request.UserName);
            }

            shoppingCart.RemoveItem(request.ProductId);

            await _basketDbContext.SaveChangesAsync(cancellationToken);

            return new RemoveItemFromBasketResult(shoppingCart.Id);
        }
    }
}
