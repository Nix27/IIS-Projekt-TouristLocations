using DAL.Model;
using System.Linq.Expressions;

namespace DAL.Repository.Abstraction
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User newUser);
        Task<User?> GetUserAsyncBy(Expression<Func<User, bool>> filter);
        Task<bool> CheckIfUsernameExists(string username);
        Task<bool> CheckIfEmailExists(string email);
        Task UpdateRefreshToken(string? refreshToken, string userEmail);
    }
}
