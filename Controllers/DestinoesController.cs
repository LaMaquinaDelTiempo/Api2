using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.DataContext;
using NuGet.Protocol;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinoesController : ControllerBase
    {
        private readonly AmadeusContext _context;

        public DestinoesController(AmadeusContext context)
        {
            _context = context;
        }


        // GET: api/destino
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Destino>>> GetDestinos()
        {
            return await _context.Destinos.ToListAsync();
        }

        // GET: api/Destinoes/by-email/user@example.com
        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<IEnumerable<Destino>>> GetDestinos(string email)
        {
            // Buscar usuario incluyendo sus preferencias
            var usuario = await _context.Usuarios
                .Include(u => u.PreferenciaUsuarios)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Obtener IDs de preferencias del usuario
            var preferenciasIds = usuario.PreferenciaUsuarios
                .Select(p => p.PreferenciasId)
                .ToList();

            // Si no tiene preferencias, devolver destinos por defecto
            if (!preferenciasIds.Any())
            {
                return await _context.Destinos
                    .Where(d => d.Id == 39 || d.Id == 40) // Usar OR en lugar de AND
                    .ToListAsync();
            }

            // Obtener destinos relacionados con las preferencias
            var destinos = await _context.DestinosPreferencias
                .Where(dp => preferenciasIds.Contains(dp.PreferenciasId))
                .Select(dp => dp.Destinos)
                .Distinct()
                .ToListAsync();

            return destinos;
        }

        /* [HttpGet("destino/{email}")]
         public async Task<ActionResult<IEnumerable<Destino>>> GetDestinos(string email)
         {
             var usuario = await _context.Usuarios.FindAsync(email);
             if (usuario == null)
             {
                 return NotFound();
             }
            return _context.Destinos;

             /*
                         var preferenciasUsuarios = await _context.PreferenciaUsuarios
                             .Where(pu => pu.UsuariosId == usuario.Id)
                             .Select(pu => pu.PreferenciasId)
                             .ToListAsync();

                         var destinos = await _context.DestinosPreferencias
                             .Where(dp => preferenciasUsuarios.Contains(dp.PreferenciasId))
                             .Select(dp => dp.Destinos)
                             .ToListAsync();
                         if (destinos == null)
                         {
                             // Reemplazar la línea con el error CS0029
                             if (destinos == null)
                             {
                                 return await _context.Destinos.Where(d => d.Id == 39 && d.Id == 40).ToListAsync();
                             }

                         }
                         return destinos; 
         }*/

        // GET: api/Destinoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Destino>> GetDestino(long id)
        {
            var destino = await _context.Destinos.FindAsync(id);

            if (destino == null)
            {
                return NotFound();
            }

            return destino;
        }

        // PUT: api/Destinoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestino(long id, Destino destino)
        {
            if (id != destino.Id)
            {
                return BadRequest();
            }

            _context.Entry(destino).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinoExists(id))
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

        // POST: api/Destinoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Destino>> PostDestino(Destino destino)
        {
            _context.Destinos.Add(destino);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDestino", new { id = destino.Id }, destino);
        }

        // DELETE: api/Destinoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestino(long id)
        {
            var destino = await _context.Destinos.FindAsync(id);
            if (destino == null)
            {
                return NotFound();
            }

            _context.Destinos.Remove(destino);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DestinoExists(long id)
        {
            return _context.Destinos.Any(e => e.Id == id);
        }
    }
}
