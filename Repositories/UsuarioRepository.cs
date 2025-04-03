﻿using Api.DataContext;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AmadeusContext _context;

        public UsuarioRepository(AmadeusContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task<Usuario?> GetByIdAsync(long id)
        {
            return await _context.Usuarios.FindAsync(id);
        }
       
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> UpdateUsuarioByEmailAsync(string email, Usuario usuario)
        {
            var existingUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUsuario == null)
                return null;

            // Mantener el ID original y actualizar solo los valores necesarios
            usuario.Id = existingUsuario.Id;
            _context.Entry(existingUsuario).CurrentValues.SetValues(usuario);

            await _context.SaveChangesAsync();
            return existingUsuario;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Usuarios.AnyAsync(u => u.Id == id);
        }

        public async Task<Usuario> RegisterUsuarioAsync(Usuario usuario)
        {
            usuario.CreatedAt = usuario.CreatedAt.HasValue
                ? DateTime.SpecifyKind(usuario.CreatedAt.Value, DateTimeKind.Unspecified)
                : DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            usuario.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<IEnumerable<Usuario>> GetAllWithPreferencesAndDestinationsAsync()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.PreferenciaUsuarios)
                    .ThenInclude(pu => pu.Preferencias)
                .ToListAsync();

            foreach (var usuario in usuarios)
            {
                var preferenciasIds = usuario.PreferenciaUsuarios
                    .Where(pu => pu.PreferenciasId.HasValue)
                    .Select(pu => pu.PreferenciasId.Value)
                    .ToList();

                if (preferenciasIds.Any())
                {
                    var destinos = await _context.DestinosPreferencias
                        .Where(dp => preferenciasIds.Contains(dp.PreferenciasId))
                        .Include(dp => dp.Destinos)
                        .Select(dp => dp.Destinos)
                        .Distinct()
                        .ToListAsync();
                }
            }

            return usuarios;
        }
    }
}
