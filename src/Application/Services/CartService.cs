using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CartService: ICartService

    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task AddProductToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            var product =  _productRepository.Get(productId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, SaleLineList = new List<SaleLine>() };
                await _cartRepository.CreateAsync(cart);
            }

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
            // Recalcular el TotalPrice después de agregar el producto
            cart.TotalPrice = cart.SaleLineList.Sum(sl => sl.SubtotalPrice);


            await _cartRepository.UpdateAsync(cart);
        }

        public async Task RemoveProductFromCartAsync(int userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
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

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                cart.SaleLineList.Clear();
                cart.TotalPrice = 0;
                await _cartRepository.UpdateAsync(cart);
            }
        }

        public async Task<double> CalculateTotalPriceAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return cart?.SaleLineList.Sum(sl => sl.SubtotalPrice) ?? 0;
        }

    }
}
