using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.DataContext;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenciaUsuariosController : ControllerBase
    {
        private readonly AmadeusContext _context;

        public PreferenciaUsuariosController(AmadeusContext context)
        {
            _context = context;
        }

        // GET: api/PreferenciaUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreferenciaUsuario>>> GetPreferenciaUsuarios()
        {
            return await _context.PreferenciaUsuarios.ToListAsync();
        }
        

        // GET: api/PreferenciaUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PreferenciaUsuario>> GetPreferenciaUsuario(long id)
        {
            var preferenciaUsuario = await _context.PreferenciaUsuarios.FindAsync(id);

            if (preferenciaUsuario == null)
            {
                return NotFound();
            }

            return preferenciaUsuario;
        }

        // PUT: api/PreferenciaUsuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreferenciaUsuario(long id, PreferenciaUsuario preferenciaUsuario)
        {
            if (id != preferenciaUsuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(preferenciaUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreferenciaUsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PreferenciaUsuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PreferenciaUsuario>> PostPreferenciaUsuario(PreferenciaUsuario preferenciaUsuario)
        {
            _context.PreferenciaUsuarios.Add(preferenciaUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPreferenciaUsuario", new { id = preferenciaUsuario.Id }, preferenciaUsuario);
        }

        // DELETE: api/PreferenciaUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreferenciaUsuario(long id)
        {
            var preferenciaUsuario = await _context.PreferenciaUsuarios.FindAsync(id);
            if (preferenciaUsuario == null)
            {
                return NotFound();
            }

            _context.PreferenciaUsuarios.Remove(preferenciaUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PreferenciaUsuarioExists(long id)
        {
            return _context.PreferenciaUsuarios.Any(e => e.Id == id);
        }
    }
}
