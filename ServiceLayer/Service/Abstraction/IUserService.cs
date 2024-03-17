using ServiceLayer.ServiceModel;

namespace ServiceLayer.Service.Abstraction
{
    public interface IUserService
    {
        Task<AuthResponse> Register(RegisterRequest registerRequest);
        Task<AuthResponse> Login(LoginRequest loginRequest);
    }
}
