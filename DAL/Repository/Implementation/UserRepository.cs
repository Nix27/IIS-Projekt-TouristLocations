using DAL.Context;
using DAL.Model;
using DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

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

        public async Task<User?> GetUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            return user;
        }
    }
}
