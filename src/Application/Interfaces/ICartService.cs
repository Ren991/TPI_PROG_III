using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task AddProductToCartAsync(int userId, int productId, int quantity);
        Task RemoveProductFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
        Task<double> CalculateTotalPriceAsync(int userId);
    }
}
