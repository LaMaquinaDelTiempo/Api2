using Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task<Usuario?> GetUsuarioByEmailAsync(string email);
        Task<Usuario> CreateUsuarioAsync(Usuario usuario);
        Task<Usuario?> UpdateUsuarioByEmailAsync(string email, Usuario usuario);
        Task<bool> DeleteUsuarioAsync(long id);
    }
}

