﻿using Application.Interfaces;
using Application.Models.CartDtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        // Obtener carrito por CartId y UserId
        public async Task<CartDto> GetCartByIdAndUserIdAsync(int cartId, int userId)
        {
            var cart = await _cartRepository.GetCartByIdAndUserIdAsync(cartId, userId);
            return CartDto.ToDto(cart);
        }

        // Crear un nuevo carrito para un usuario
        public async Task<CartDto> CreateCartForUserAsync(int userId)
        {
            var cart = new Cart { UserId = userId, SaleLineList = new List<SaleLine>(), TotalPrice = 0 };

            var createdCart = await _cartRepository.CreateAsync(cart);
            return CartDto.ToDto(createdCart);
        }

        // Añadir producto al carrito específico del usuario
        public async Task AddProductToCartAsync(int userId, int productId, int quantity)
        {// En realidad se almacena en el localStorage.

            // Obtener todos los carritos del usuario
            var carts = await _cartRepository.GetCartByUserIdAsync(userId);

            // Buscar el primer carrito no pagado
            var cart = carts.FirstOrDefault(c => !c.IsPayabled);

            // Si no existe un carrito no pagado, crear uno nuevo
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepository.CreateAsync(cart);
            }

            // Agregar el producto al carrito (código existente, adaptado)
            var product = _productRepository.Get(productId);

            var saleLine = cart.SaleLineList.FirstOrDefault(sl => sl.ProductId == productId);
            if (saleLine != null)
            {
                saleLine.Quantity += quantity;
                saleLine.SubtotalPrice = saleLine.Quantity * product.Price;
            }
            else
            {
                cart.SaleLineList.Add(new SaleLine
                {
                    ProductId = productId,
                    Product = product,
                    Quantity = quantity,
                    SubtotalPrice = quantity * product.Price
                });
            }

            // Recalcular el TotalPrice del carrito
            cart.TotalPrice = cart.SaleLineList.Sum(sl => sl.SubtotalPrice);

            // Actualizar el carrito en la base de datos
            await _cartRepository.UpdateAsync(cart);
        }

        // Eliminar un producto del carrito específico del usuario
        public async Task RemoveProductFromCartAsync(int userId, int cartId, int productId)
        {
            var cart = await _cartRepository.GetCartByIdAndUserIdAsync(cartId, userId);
            if (cart != null)
            {
                var saleLine = cart.SaleLineList.FirstOrDefault(sl => sl.ProductId == productId);
                if (saleLine != null)
                {
                    cart.SaleLineList.Remove(saleLine);
                    cart.TotalPrice = cart.SaleLineList.Sum(sl => sl.SubtotalPrice);
                    await _cartRepository.UpdateAsync(cart);
                }
            }
        }

        // Vaciar un carrito específico del usuario
        public async Task ClearCartAsync(int userId, int cartId)
        {
            var cart = await _cartRepository.GetCartByIdAndUserIdAsync(cartId, userId);
            if (cart != null)
            {
                cart.SaleLineList.Clear();
                cart.TotalPrice = 0;
                await _cartRepository.UpdateAsync(cart);
            }
        }

        // Calcular el precio total de un carrito específico del usuario
        public async Task<double> CalculateTotalPriceAsync(int userId, int cartId)
        {
            var cart = await _cartRepository.GetCartByIdAndUserIdAsync(cartId, userId);
            return cart?.SaleLineList.Sum(sl => sl.SubtotalPrice) ?? 0;
        }

        // Obtener todos los carritos de un usuario
        public async Task<List<CartDto>> GetCartsByUserIdAsync(int userId)
        {
            var carts = await _cartRepository.GetCartByUserIdAsync(userId);

            var cartDtos = carts.Select(cart => CartDto.ToDto(cart)).ToList();

            return cartDtos;        
        }

        public async Task PayCartAsync(int userId, int cartId, TypePayment typePayment)
        {
            // Buscar el carrito del usuario
            var cart = await _cartRepository.GetCartByIdAndUserIdAsync(cartId, userId);
            if (cart == null)
            {
                throw new Exception("Carrito no encontrado.");
            }

            if (cart.IsPayabled)
            {
                throw new Exception("El carrito ya ha sido pagado.");
            }

            // Cambiar la propiedad IsPayabled a true y asignar el método de pago
            cart.IsPayabled = true;
            cart.TypePayment = typePayment;

            // Guardar los cambios en el repositorio
            await _cartRepository.UpdateAsync(cart);

            foreach (var saleLine in cart.SaleLineList)
            {
                var product =  _productRepository.Get(saleLine.ProductId); 
                if (product != null)
                {
                    product.Stock -= saleLine.Quantity; // Restar la cantidad vendida
                    _productRepository.Update(product); // Actualizar el producto
                }
            }
        }

        public async Task<List<CartDto>> GetPaidCartsByUserIdAsync(int userId)
        {
            var carts = await _cartRepository.GetPaidCartsByUserIdAsync(userId);

            var cartDtos = carts.Select(cart => CartDto.ToDto(cart)).ToList();

            return cartDtos;
        }


    }
}
