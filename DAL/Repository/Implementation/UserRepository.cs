using DAL.Context;
using DAL.Model;
using DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository.Implementation
{
    public class UserRepository(ApplicationContext context) : IUserRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task<bool> CheckIfEmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.Equals(email));
        }

        public async Task<bool> CheckIfUsernameExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName.Equals(username));
        }

        public async Task CreateUserAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        public Task<User?> GetUserAsyncBy(Expression<Func<User, bool>> filter)
        {
            return _context.Users.FirstOrDefaultAsync(filter);
        }

        public async Task UpdateRefreshToken(string? refreshToken, string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userEmail));

            if (refreshToken != null)
            {
                user!.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
            }
            else
            {
                user!.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
            }
        }
    }
}
