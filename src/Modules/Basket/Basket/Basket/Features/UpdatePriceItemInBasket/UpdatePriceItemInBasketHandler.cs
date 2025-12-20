using Basket.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.CQRS;

namespace Basket.Basket.Features.UpdatePriceItemInBasket
{

    public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price) : ICommand<UpdateItemPriceInBasketResult>;

    public record UpdateItemPriceInBasketResult(bool IsSuccess);

    public class UpdatePriceItemInBasketHandler : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
    {
        private readonly BasketDbContext _basketDbContext;

        public UpdatePriceItemInBasketHandler(BasketDbContext basketDbContext)
        {
            _basketDbContext = basketDbContext;
        }

        public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand request, CancellationToken cancellationToken)
        {
            var itemsToUpdate = await _basketDbContext.ShoppingCartItems
                .Where(x => x.ProductId == request.ProductId)
                .ToListAsync(cancellationToken);

            if (itemsToUpdate.Count == 0)
            {
                return new UpdateItemPriceInBasketResult(false);
            }

            foreach (var item in itemsToUpdate)
            {
                item.UpdatePrice(item.Price);
            }

            await _basketDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateItemPriceInBasketResult(true);
        }
    }
}
