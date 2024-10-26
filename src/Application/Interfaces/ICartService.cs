using Application.Models.CartDtos;
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
        Task<CartDto> CreateCartForUserAsync(int userId);
        Task<List<CartDto>> GetPaidCartsByUserIdAsync(int userId);
        Task AddProductToCartAsync(int userId, int productId, int quantity);
        Task RemoveProductFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
        Task<double> CalculateTotalPriceAsync(int userId);
        Task PayCartAsync(int userId, TypePayment typePayment);
        Task<List<CartDto>> GetCartsByUserIdAsync(int userId);
    }
}
