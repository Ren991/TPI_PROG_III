﻿using Application.Interfaces;
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

        [HttpPost]
        [Authorize]

        public IActionResult AddProduct([FromBody] ProductCreateRequest product)

        {
            var userTypeString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userTypeString == "Admin")
            {
                var newProduct = _productService.AddNewProduct(product);
                return Ok(newProduct);
            }
            else
            {
                return Forbid();
            }
        }

        [HttpDelete]
        [Authorize]

        public IActionResult DeleteProduct([FromBody] int productId)
        {
            var userTypeString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userTypeString == "Admin")
            {

                _productService.DeleteProduct(productId);
            return Ok(new { message = "Product deleted successfully." });
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateProduct(int id, [FromQuery] string description, [FromQuery] double price, [FromQuery] int stock)
        {
            var userTypeString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userTypeString == "Admin")
            {
                _productService.UpdateProduct(id, description, price, stock);
                return Ok(new { message = "Producto actualizado exitosamente." });
            }
            else
            {
                return Forbid();
            }

        }

    }
}
