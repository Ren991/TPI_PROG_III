using Application.Interfaces;
using Application.Models.UserDtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<ICollection<UserResponse>> GetAllUsers()
        {
            try
            {
                return Ok(_userService.GetAllUsers());
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
