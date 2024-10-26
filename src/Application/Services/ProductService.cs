using Application.Interfaces;
using Application.Models.ProductDtos;
using Application.Models.UserDtos;
using Domain.Entities;
using Domain.Exceptions;
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



        public void UpdateProduct(int id, ProductCreateRequest productDto)
        {
            Product? product = _productRepository.Get(id);
            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.Category = productDto.Category;

            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            Product? product = _productRepository.Get(id);
            
            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }
            _productRepository.Delete(product);
        }
    }
}
