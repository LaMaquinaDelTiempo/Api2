using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (token, userType) = await _authService.Authenticate(request);

            if (token == null)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            // Configurar la respuesta con encabezados adecuados para CORS
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

            return Ok(new { token, userType });
        }

        // Agregar un método OPTIONS para manejar preflight requests
        [HttpOptions("login")]
        [EnableCors("AllowAll")]
        public IActionResult PreflightLogin()
        {
            return NoContent();
        }
    }
}
