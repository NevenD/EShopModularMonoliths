using Basket.Basket.Modules;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Data.Repository
{
    public class CachedBasketRepository : IBasketRepository
    {
        private readonly IBasketRepository _repository;
        private readonly IDistributedCache _distributedCache;

        public CachedBasketRepository(IBasketRepository repository, IDistributedCache distributedCache)
        {
            _repository = repository;
            _distributedCache = distributedCache;
        }

        public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            if (!asNoTracking)
            {
                return await _repository.GetBasket(userName, false, cancellationToken);
            }

            var cachedBasket = await _distributedCache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }

            var basket = await _repository.GetBasket(userName, asNoTracking, cancellationToken);
            await _distributedCache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await _repository.CreateBasket(basket, cancellationToken);

            await _distributedCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteBasket(userName, cancellationToken);
            await _distributedCache.RefreshAsync(userName, cancellationToken);

            return true;
        }

        public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
        {

            var result = await _repository.SaveChangesAsync(userName, cancellationToken);

            if (userName is not null)
            {
                await _distributedCache.RefreshAsync(userName, cancellationToken);
            }

            return result;
        }
    }
}
