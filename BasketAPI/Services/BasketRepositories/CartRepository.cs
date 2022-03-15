using CartAPI.Services.BasketRepositories.Interfaces;
using CartAPI.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


namespace CartAPI.Services.BasketRepositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDistributedCache _redisCache;
        public CartRepository(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<Cart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(basket))
                return null;
            return JsonConvert.DeserializeObject<Cart>(basket);
        }

        public async Task<Cart> UpdateBasket(Cart cart)
        {
            await _redisCache.SetStringAsync(cart.UserName, JsonConvert.SerializeObject(cart));

            return await GetBasket(cart.UserName);
        }
    }
}
