using Application.Interfaces;
using Application.Models.ProductDtos;
using Application.Models.UserDtos;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]

        public IActionResult GetAll()
        {

            var products = _productService.GetAllProducts();

            return Ok(products);
        }

        [Authorize(Roles= "SuperAdmin, Admin")]
        [HttpPost]
        
        public IActionResult AddProduct([FromBody] ProductCreateRequest product)

        {
                var newProduct = _productService.AddNewProduct(product);
                return Ok(newProduct);
        
        }

        [Authorize("SuperAdmin")]
        [HttpDelete]

        public IActionResult DeleteProduct([FromBody] int productId)
        {
            _productService.DeleteProduct(productId);
            return Ok(new { message = "Product deleted successfully." });
            
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPut("{id}")]
        
        public IActionResult UpdateProduct(int id, [FromQuery] string description, [FromQuery] double price, [FromQuery] int stock)
        {
            _productService.UpdateProduct(id, description, price, stock);
            return Ok(new { message = "Product successfully updated." });

        }

    }
}
