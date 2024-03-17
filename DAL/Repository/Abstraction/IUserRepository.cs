using DAL.Model;
using System.Linq.Expressions;

namespace DAL.Repository.Abstraction
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User newUser);
        Task<User?> GetUserAsync(string email);
        Task<bool> CheckIfUsernameExists(string username);
        Task<bool> CheckIfEmailExists(string email);
    }
}
