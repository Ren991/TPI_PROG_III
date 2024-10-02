using Application.Models.UserDtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.ProductDtos
{
    public class ProductDto
    {

        public string Name { get; set; }

        public int Stock { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Category { get; set; }

        public static ProductDto ToDto(Product product)
        {
            ProductDto productDto = new();
            productDto.Name = product.Name;
            productDto.Stock = product.Stock;
            productDto.Price = product.Price;
            productDto.Description = product.Description;
            productDto.Image = product.Image;
            productDto.Category = product.Category; 

            return productDto;

        }
    }
}
