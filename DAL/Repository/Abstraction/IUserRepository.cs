using DAL.Model;

namespace DAL.Repository.Abstraction
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User newUser);
        Task<User?> GetUserAsync(string email, string password);
    }
}
