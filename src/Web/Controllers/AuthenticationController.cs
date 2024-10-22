﻿using Application.Interfaces;
using Application.Models.AuthDtos;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthenticationService _customAuthenticationService;

        public AuthenticationController(IConfiguration config, IAuthenticationService autenticacionService)
        {
            _config = config;
            _customAuthenticationService = autenticacionService;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Autenticar(UserLoginRequest loginRequest)
        {
            string token = _customAuthenticationService.AuthenticateAsync(loginRequest);

            if (token == null)
            {
                throw new Exception("Unauthenticated user.");
            }
            return Ok(token );
        }
    }
}
