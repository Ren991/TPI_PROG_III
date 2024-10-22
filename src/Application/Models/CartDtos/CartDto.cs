using Application.Models.SaleLineDtos;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.CartDtos
{
    public class CartDto
    {
        public int Id { get; set; }

        public double TotalPrice { get; set; }

        public TypePayment TypePayment { get; set; }

        public bool IsPayabled { get; set; }

        public int UserId { get; set; }

        public ICollection<CartLineDto> SaleLineList { get; set; } = new List<CartLineDto>();

        public static CartDto ToDto(Cart cart)
        {
            CartDto cartDto = new CartDto();
            cartDto.Id = cart.Id;
            cartDto.TotalPrice = cart.TotalPrice;
            cartDto.TypePayment = cart.TypePayment;
            cartDto.IsPayabled = cart.IsPayabled;
            cartDto.UserId = cart.UserId;
            cartDto.SaleLineList = CartLineDto.ToCollectionDto(cart.SaleLineList); // Assuming a separate SaleLineDto exists

            return cartDto;
        }
    }
}
