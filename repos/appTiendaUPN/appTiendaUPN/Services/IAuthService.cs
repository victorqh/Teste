using appTiendaUPN.Models;

namespace appTiendaUPN.Services
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string email, string password);
        Task<User> RegisterUserAsync(RegisterViewModel model);
        Task<bool> EmailExistsAsync(string email);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
