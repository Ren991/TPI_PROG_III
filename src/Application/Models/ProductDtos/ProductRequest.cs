using Application.Models.UserDtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.ProductDtos
{
    public class ProductCreateRequest
    {

        public string Name { get; set; }

        public int Stock { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

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
    }
}
