﻿using Api.Models;
using Api.Repositories;
using BCrypt.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IDestinoRepository _destinoRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IDestinoRepository destinoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _destinoRepository = destinoRepository;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(long id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            return await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task<Usuario?> UpdateUsuarioByEmailAsync(string email, Usuario usuario)
        {
            return await _usuarioRepository.UpdateUsuarioByEmailAsync(email, usuario);
        }

        public async Task<bool> DeleteUsuarioAsync(long id)
        {
            return await _usuarioRepository.DeleteAsync(id);
        }

        public async Task<Usuario> RegisterUsuarioAsync(Usuario usuario)
        {
            usuario.Password = HashPassword(usuario.Password);
            usuario.UserType = string.IsNullOrEmpty(usuario.UserType) ? "CLIENT" : usuario.UserType;

            return await _usuarioRepository.RegisterUsuarioAsync(usuario);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<IEnumerable<object>> GetAllUsuariosWithPreferencesAndDestinationsAsync()
        {
            var usuarios = await _usuarioRepository.GetAllWithPreferencesAndDestinationsAsync();
            var result = new List<object>();

            foreach (var usuario in usuarios)
            {
                var preferenciasIds = usuario.PreferenciaUsuarios
                    .Where(pu => pu.PreferenciasId.HasValue)
                    .Select(pu => pu.PreferenciasId.Value)
                    .ToList();

                var destinos = new List<Destino>();
                if (preferenciasIds.Any())
                {
                    foreach (var prefId in preferenciasIds)
                    {
                        var usuarioEmail = usuario.Email;
                        var destinosParaPreferencia = await _destinoRepository.GetDestinosByEmailAsync(usuarioEmail);
                        destinos.AddRange(destinosParaPreferencia);
                    }
                    
                    destinos = destinos.Distinct().ToList();
                }

                var preferencias = usuario.PreferenciaUsuarios
                    .Where(pu => pu.Preferencias != null)
                    .Select(pu => new
                    {
                        Id = pu.Preferencias.Id,
                        Entorno = pu.Preferencias.Entorno,
                        Clima = pu.Preferencias.Clima,
                        Actividad = pu.Preferencias.Actividad,
                        Alojamiento = pu.Preferencias.Alojamiento,
                        TiempoViaje = pu.Preferencias.TiempoViaje,
                        RangoEdad = pu.Preferencias.RangoEdad
                    }).ToList();

                result.Add(new
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    UserType = usuario.UserType,
                    Preferencias = preferencias,
                    Destinos = destinos
                });
            }

            return result;
        }
    }
}
