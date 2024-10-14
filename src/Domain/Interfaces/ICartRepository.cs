using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByIdAndUserIdAsync(int cartId, int userId);
        Task<List<Cart>> GetCartByUserIdAsync(int userId);
        Task CreateAsync(Cart cart);        
        Task UpdateAsync(Cart cart);

        Task<List<Cart>> GetPaidCartsByUserIdAsync(int userId);

    }
}
