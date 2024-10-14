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

        public async Task<Cart> GetCartByIdAndUserIdAsync(int cartId, int userId)
        {
            return await _context.Carts
                .Include(c => c.SaleLineList) // Incluir las líneas de venta del carrito
                .FirstOrDefaultAsync(c => c.Id == cartId && c.UserId == userId);
        }

        // Obtener todos los carritos de un usuario por userId
        public async Task<List<Cart>> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.SaleLineList) // Incluir las líneas de venta de cada carrito
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        // Crear un nuevo carrito
        public async Task CreateAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        // Actualizar un carrito existente
        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cart>> GetPaidCartsByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.SaleLineList)
                .Where(cart => cart.IsPayabled && cart.UserId == userId)
                .ToListAsync();
        }

    }
}
