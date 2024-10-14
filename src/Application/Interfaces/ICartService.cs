using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartByIdAndUserIdAsync(int cartId, int userId);
        Task<Cart> CreateCartForUserAsync(int userId);
        Task<List<Cart>> GetPaidCartsByUserIdAsync(int userId);
        Task AddProductToCartAsync(int userId, int cartId, int productId, int quantity);
        Task RemoveProductFromCartAsync(int userId, int cartId, int productId);
        Task ClearCartAsync(int userId, int cartId);
        Task<double> CalculateTotalPriceAsync(int userId, int cartId);
        Task PayCartAsync(int userId, int cartId, TypePayment typePayment);
        Task<List<Cart>> GetCartsByUserIdAsync(int userId);
    }
}
