﻿using Domain.Entities;
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

        // Obtener todos los carritos de un usuario por userId
        public async Task<List<Cart>> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartLineList) // Incluir las líneas de venta de cada carrito
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        // Obtener todos los carritos de los usuarios
        public async Task<List<Cart>> GetAllCarts()
        {
            return await _context.Carts
                .Include(c => c.CartLineList) // Incluir las líneas de venta de cada carrito
                .ToListAsync();
        }

        // Crear un nuevo carrito
        public async Task<Cart> CreateAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        // Actualizar un carrito existente
        public async Task<Cart> UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<List<Cart>> GetPaidCartsByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartLineList)
                .Where(cart => cart.IsPayabled && cart.UserId == userId)
                .ToListAsync();
        }

    }
}
