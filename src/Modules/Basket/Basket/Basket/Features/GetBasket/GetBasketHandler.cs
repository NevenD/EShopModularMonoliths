using Basket.Basket.Dtos;
using Basket.Basket.Exceptions;
using Basket.Data.Repository;
using Mapster;
using Shared.Contracts.CQRS;

namespace Basket.Basket.Features.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCartDto ShoppingCart);

    public sealed class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        private readonly IBasketRepository _basketRepository;

        public GetBasketHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetBasket(request.UserName, true, cancellationToken);

            if (basket is null)
            {
                throw new BasketNotFoundException(request.UserName);
            }

            var basketDto = basket.Adapt<ShoppingCartDto>();

            return new GetBasketResult(basketDto);
        }
    }
}
