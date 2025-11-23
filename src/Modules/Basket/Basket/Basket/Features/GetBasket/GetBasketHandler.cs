using Basket.Basket.Dtos;
using Basket.Basket.Exceptions;
using Basket.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Basket.Features.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCartDto ShoppingCart);

    public sealed class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        private readonly BasketDbContext _basketDbContext;

        public GetBasketHandler(BasketDbContext basketDbContext)
        {
            _basketDbContext = basketDbContext;
        }

        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _basketDbContext.ShoppingCarts
                .AsNoTracking()
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

            if (basket is null)
            {
                throw new BasketNotFoundException(request.UserName);
            }

            var basketDto = basket.Adapt<ShoppingCartDto>();

            return new GetBasketResult(basketDto);
        }
    }
}
