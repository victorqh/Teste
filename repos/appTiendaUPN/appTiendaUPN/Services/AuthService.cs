using appTiendaUPN.Models;
using appTiendaUPN.Repositories;
using BCrypt.Net;

namespace appTiendaUPN.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            
            if (user == null)
                return null;

            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<User> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new User
            {
                Nombre = model.Nombre,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Telefono = model.Telefono,
                Direccion = model.Direccion,
                Rol = "Cliente",
                FechaRegistro = DateTime.UtcNow
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
