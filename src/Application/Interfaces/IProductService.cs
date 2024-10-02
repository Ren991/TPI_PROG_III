using Application.Models.ProductDtos;
using Application.Models.UserDtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        ProductDto AddNewProduct(ProductCreateRequest productDto);

        void UpdateProduct(int id, string description, double price, int stock);

        void DeleteProduct(int id);
    }
}
