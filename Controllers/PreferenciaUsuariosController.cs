using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenciaUsuariosController : ControllerBase
    {
        private readonly IPreferenciaUsuarioService _preferenciaUsuarioService;

        public PreferenciaUsuariosController(IPreferenciaUsuarioService preferenciaUsuarioService)
        {
            _preferenciaUsuarioService = preferenciaUsuarioService;
        }

        // GET: api/PreferenciaUsuarios
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreferenciaUsuario>>> GetPreferenciaUsuarios()
        {
            var result = await _preferenciaUsuarioService.GetAllPreferenciaUsuariosAsync();
            return Ok(result);
        }
        [Authorize] // Solo usuarios autenticados pueden acceder
        // GET: api/PreferenciaUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PreferenciaUsuario>> GetPreferenciaUsuario(long id)
        {
            var result = await _preferenciaUsuarioService.GetPreferenciaUsuarioByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [Authorize] // Solo usuarios autenticados pueden acceder
        // POST: api/PreferenciaUsuarios
        [HttpPost]
        public async Task<ActionResult<PreferenciaUsuario>> PostPreferenciaUsuario(PreferenciaUsuario preferenciaUsuario)
        {
            var created = await _preferenciaUsuarioService.CreatePreferenciaUsuarioAsync(preferenciaUsuario);
            return CreatedAtAction(nameof(GetPreferenciaUsuario), new { id = created.Id }, created);
        }
        [Authorize] // Solo usuarios autenticados pueden acceder
        // PUT: api/PreferenciaUsuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreferenciaUsuario(long id, PreferenciaUsuario preferenciaUsuario)
        {
            if (id != preferenciaUsuario.Id)
                return BadRequest();

            var updated = await _preferenciaUsuarioService.UpdatePreferenciaUsuarioAsync(preferenciaUsuario);
            if (updated == null)
                return NotFound();

            return NoContent();
        }
        [Authorize] // Solo usuarios autenticados pueden acceder
        // DELETE: api/PreferenciaUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreferenciaUsuario(long id)
        {
            var deleted = await _preferenciaUsuarioService.DeletePreferenciaUsuarioAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
