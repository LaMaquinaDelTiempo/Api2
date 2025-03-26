﻿using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (token, userType) = await _authService.Authenticate(request);

            if (token == null)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            return Ok(new { token, userType }); // Se devuelve el token y el userType
        }
    }
}
