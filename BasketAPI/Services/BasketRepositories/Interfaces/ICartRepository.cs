using CartAPI.Entities;
using System.Threading.Tasks;

namespace CartAPI.Services.BasketRepositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetBasket(string userName);
        Task<Cart> UpdateBasket(Cart cart);
        Task DeleteBasket(string userName);
    }
}
