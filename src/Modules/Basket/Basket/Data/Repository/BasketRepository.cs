using Basket.Basket.Exceptions;
using Basket.Basket.Modules;
using Microsoft.EntityFrameworkCore;

namespace Basket.Data.Repository
{
    public sealed class BasketRepository : IBasketRepository
    {
        private readonly BasketDbContext _basketDbContext;

        public BasketRepository(BasketDbContext basketDbContext)
        {
            _basketDbContext = basketDbContext;
        }

        public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            var query = _basketDbContext.ShoppingCarts.Include(x => x.Items).Where(x => x.UserName == userName);

            if (asNoTracking)
            {
                query.AsNoTracking();
            }

            var basket = await query.SingleOrDefaultAsync(cancellationToken);

            return basket ?? throw new BasketNotFoundException(userName);
        }

        public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            _basketDbContext.ShoppingCarts.Add(basket);
            await _basketDbContext.SaveChangesAsync(cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await GetBasket(userName, false, cancellationToken);
            _basketDbContext.ShoppingCarts.Remove(basket);
            await _basketDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
        {
            return await _basketDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
