using Application.Interfaces;
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
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        
        public IActionResult GetAll()
        {
            

            return Ok(_userService.GetAllUsers());
        }

        [HttpPost]
        
        public IActionResult AddUser([FromBody] UserCreateRequest user)

        {
            var newUser = _userService.AddNewUser(user);
            return Ok(newUser);            
        }

        [HttpGet("/email")]
        
        public IActionResult GetByEmail([FromQuery] string email)
        { 
          return Ok(_userService.GetUserByEmail(email));
        }

        [HttpPut("/password")]
        
        public IActionResult UpdateUser([FromBody] string password)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userTypeString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userIdClaim == null)
            {
                throw new Exception("Usuario no autenticado.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Invalid user ID format.");
            }

            _userService.UpdateUser(userId, password);

            return Ok(new { message = "Password updated successfully." });
        }

        [HttpDelete]
       
        public IActionResult DeleteUser([FromBody] int userId)
        {
            
            _userService.DeleteUser(userId);
            return Ok(new { message = "User deleted successfully." });
        }
    }
}
