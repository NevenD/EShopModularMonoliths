using Basket.Basket.Exceptions;
using Basket.Data;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;

    public record DeleteBasketResult(bool IsSuccess);

    public sealed class DeleteBasketHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        private readonly BasketDbContext _basketDbContext;

        public DeleteBasketHandler(BasketDbContext basketDbContext)
        {
            _basketDbContext = basketDbContext;
        }

        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await _basketDbContext.ShoppingCarts.SingleOrDefaultAsync(x => x.UserName == command.Username, cancellationToken);

            if (basket is null)
            {
                throw new BasketNotFoundException(command.Username);
            }

            _basketDbContext.ShoppingCarts.Remove(basket);
            await _basketDbContext.SaveChangesAsync(cancellationToken);

            return new DeleteBasketResult(true);

        }
    }
}
