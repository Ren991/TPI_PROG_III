using Application.Interfaces;
using Application.Models.ProductDtos;
using Application.Models.UserDtos;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProducts()
        {
            var users = _productRepository.Get();
            return users;
        }

        public ProductDto AddNewProduct(ProductCreateRequest productDto)
        {
            
            return ProductDto.ToDto(_productRepository.Create(ProductCreateRequest.ToEntity(productDto)));
        }



        public void UpdateProduct(int id, string description, double price, int stock)
        {
            Product? product = _productRepository.Get(id);
            if (product == null)
            {
                throw new Exception("No se encontró el producto");
            }

            product.Description = description;
            product.Price = price;
            product.Stock = stock;

            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            Product? product = _productRepository.Get(id);
            
            if (product == null)
            {
                throw new Exception("No se encontró el producto");
            }
            _productRepository.Delete(product);
        }
    }
}
