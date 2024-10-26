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
        Task<List<Cart>> GetCartByUserIdAsync(int userId);
        Task<Cart> CreateAsync(Cart cart);        
        Task<Cart> UpdateAsync(Cart cart);

        Task<List<Cart>> GetPaidCartsByUserIdAsync(int userId);

    }
}
