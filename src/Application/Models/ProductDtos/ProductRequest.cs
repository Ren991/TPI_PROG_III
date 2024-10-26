using Application.Models.UserDtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.ProductDtos
{
    public class ProductCreateRequest
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Category { get; set; }

        public static Product ToEntity(ProductCreateRequest productDto)
        {
            Product product = new Product();
            product.Name = productDto.Name;
            product.Stock = productDto.Stock;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.Image = productDto.Image;
            product.Category = productDto.Category;

            return product;

        }

        public static bool validateDto(ProductCreateRequest dto)
        {
            if (dto.Name == default ||
                dto.Price == default ||
                dto.Description == default ||
                dto.Image == default ||
                dto.Category == default ||
                dto.Stock == default)
                return false;

            return true;
        }
    }
}
