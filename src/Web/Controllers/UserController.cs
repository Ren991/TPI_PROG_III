using Application.Interfaces;
using Application.Models.UserDtos;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
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

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        
        public IActionResult GetAll()
        {
            
            var users = _userService.GetAllUsers();

            return Ok(users);
        }

        [HttpPost]
        
        public IActionResult AddUser([FromBody] UserCreateRequest user) // Este endpoint es para crear usuario comunes.

        {
            var newUser = _userService.AddNewUser(user);
            return Ok(newUser);            
        }

        [Authorize("SuperAdmin")]
        [HttpPost("/create-admin")]

        public IActionResult AddAdminUser([FromBody] UserAdminCreateRequest user) // Este endpoint es para crear usuario Admin.

        {
            var newUser = _userService.AddNewAdminUser(user);
            return Ok(newUser);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("/email")]
        
        public IActionResult GetByEmail([FromQuery] string email)
        { 
          return Ok(_userService.GetUserByEmail(email));
        }

        [Authorize]
        [HttpPut("/password")]
        
        public IActionResult UpdateUser([FromBody] string password)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);


            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new BadRequestException("Invalid user ID format.");
            }

            _userService.UpdateUser(userId, password);

            return Ok(new { message = "Password updated successfully." });
        }

        [Authorize("SuperAdmin")]
        [HttpDelete]
       
        public IActionResult DeleteUser([FromBody] int userId)
        {
            
            _userService.DeleteUser(userId);
            return Ok(new { message = "User deleted successfully." });
        }
    }
}
