
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository repository,IDistributedCache cache) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            // Getting / Checking data from IDistributed Cache...
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if(!string.IsNullOrEmpty(cachedBasket)) 
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            // If NULL we have to get data from database and set into Cache...
            var basket = await repository.GetBasket(userName, cancellationToken);
            await cache.SetStringAsync(userName,JsonSerializer.Serialize(basket));

            return basket;
        }
        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await repository.StoreBasket(basket, cancellationToken);

            await cache.SetStringAsync(basket.UserName,JsonSerializer.Serialize(basket));

            return basket;
        }
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);

            await cache.RemoveAsync(userName,cancellationToken);

            return true;
        }
    }
}
