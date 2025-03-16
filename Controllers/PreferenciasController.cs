using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.DataContext;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenciasController : ControllerBase
    {
        private readonly AmadeusContext _context;

        public PreferenciasController(AmadeusContext context)
        {
            _context = context;
        }

        // GET: api/Preferencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Preferencia>>> GetPreferencias()
        {
            return await _context.Preferencias.ToListAsync();
        }

        // GET: api/Preferencias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Preferencia>> GetPreferencia(long id)
        {
            var preferencia = await _context.Preferencias.FindAsync(id);

            if (preferencia == null)
            {
                return NotFound();
            }

            return preferencia;
        }

        // PUT: api/Preferencias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreferencia(long id, Preferencia preferencia)
        {
            if (id != preferencia.Id)
            {
                return BadRequest();
            }

            _context.Entry(preferencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreferenciaExists(id))
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

        // POST: api/Preferencias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Preferencia>> PostPreferencia(Preferencia preferencia)
        {
            _context.Preferencias.Add(preferencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPreferencia", new { id = preferencia.Id }, preferencia);
        }

        // DELETE: api/Preferencias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreferencia(long id)
        {
            var preferencia = await _context.Preferencias.FindAsync(id);
            if (preferencia == null)
            {
                return NotFound();
            }

            _context.Preferencias.Remove(preferencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PreferenciaExists(long id)
        {
            return _context.Preferencias.Any(e => e.Id == id);
        }
    }
}
