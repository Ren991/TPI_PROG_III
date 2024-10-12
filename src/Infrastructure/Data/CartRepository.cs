using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CartRepository: ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            var cart = await _context.Carts
                             .Include(c => c.SaleLineList)
                             .ThenInclude(sl => sl.Product)
                             .FirstOrDefaultAsync(c => c.UserId == userId);

            // Si el carrito existe, recalculamos el TotalPrice
            if (cart != null)
            {
                cart.TotalPrice = cart.SaleLineList.Sum(sl => sl.SubtotalPrice);
            }

            return cart;
        }

        public async Task CreateAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

    }
}
