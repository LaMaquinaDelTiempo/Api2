using Api.Models;
using Api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            // Aquí podrías agregar validaciones o lógica adicional antes de crear el usuario.
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
    }
}

