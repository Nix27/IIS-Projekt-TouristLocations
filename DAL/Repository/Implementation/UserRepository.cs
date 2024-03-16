using DAL.Context;
using DAL.Model;
using DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Implementation
{
    internal class UserRepository(ApplicationContext context) : IUserRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task CreateUserAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;
        }
    }
}
