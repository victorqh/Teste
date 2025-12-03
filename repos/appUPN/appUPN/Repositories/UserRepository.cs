using appUPN.Data;
using appUPN.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace appUPN.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> ObtenerPorEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> CrearUsuarioAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> ValidarCredencialesAsync(string email, string password)
        {
            var user = await ObtenerPorEmailAsync(email);
            
            if (user == null)
                return null;

            // Verificar password con BCrypt
            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return user;

            return null;
        }
    }
}
