using appUPN.Models;

namespace appUPN.Repositories
{
    public interface IUserRepository
    {
        Task<User?> ObtenerPorEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email);
        Task<User> CrearUsuarioAsync(User user);
        Task<User?> ValidarCredencialesAsync(string email, string password);
    }
}
