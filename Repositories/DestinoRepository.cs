using Api.DataContext;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public class DestinoRepository : IDestinoRepository
    {
        private readonly AmadeusContext _context;

        public DestinoRepository(AmadeusContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Destino>> GetAllAsync()
        {
            return await _context.Destinos.ToListAsync();
        }

        public async Task<Destino?> GetByIdAsync(long id)
        {
            return await _context.Destinos.FindAsync(id);
        }

        public async Task<Destino> CreateAsync(Destino destino)
        {
            _context.Destinos.Add(destino);
            await _context.SaveChangesAsync();
            return destino;
        }

        public async Task<Destino?> UpdateAsync(Destino destino)
        {
            if (!await _context.Destinos.AnyAsync(d => d.Id == destino.Id))
                return null;

            _context.Entry(destino).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return destino;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var destino = await _context.Destinos.FindAsync(id);
            if (destino == null)
                return false;

            _context.Destinos.Remove(destino);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Destino>> GetDestinosByEmailAsync(string email)
        {
            // Buscar el usuario con sus preferencias
            var usuario = await _context.Usuarios
                .Include(u => u.PreferenciaUsuarios)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return new List<Destino>();

            // Obtener los IDs de las preferencias del usuario
            var preferenciasIds = usuario.PreferenciaUsuarios
                .Select(pu => pu.PreferenciasId)
                .Where(id => id != null)
                .Select(id => id.Value)
                .ToList();

            // Si no tiene preferencias, devolver destinos por defecto (por ejemplo, con IDs fijos)
            if (!preferenciasIds.Any())
            {
                return await _context.Destinos
                    .Where(d => d.Id == 39 || d.Id == 40)
                    .ToListAsync();
            }

            // Obtener destinos relacionados a las preferencias del usuario
            var destinos = await _context.DestinosPreferencias
.Where(dp => preferenciasIds.Contains(dp.PreferenciasId))
                .Select(dp => dp.Destinos)
                .Distinct()
                .ToListAsync();

            return destinos;
        }
    }
}
