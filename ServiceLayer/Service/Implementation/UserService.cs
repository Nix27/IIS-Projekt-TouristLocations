using DAL.Repository.Abstraction;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.ServiceModel;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using ServiceLayer.Provider;
using Microsoft.Extensions.Configuration;

namespace ServiceLayer.Service.Implementation
{
    public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IConfiguration configuration) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        public async Task<AuthResponse> Login(LoginRequest loginRequest)
        {
            if(!await Authenticate(loginRequest.Email, loginRequest.Password))
            {
                return new AuthResponse { IsSuccessful = false, Message = "Invalid email or password" };
            }

            return new LoginResponse
            {
                IsSuccessful = true,
                Message = "Login successful",
                Tokens = new JwtTokenProvider(_configuration).GenerateTokens(new JwtTokenBodyInfo
                {
                    Email = loginRequest.Email
                })
            };
        }

        private async Task<bool> Authenticate(string email, string password)
        {
            if(!ValidateEmail(email)) return false;

            var user = await _userRepository.GetUserAsync(email);
            
            if(user == null) return false;

            byte[] salt = Convert.FromBase64String(user.PwdSalt);
            byte[] hash = Convert.FromBase64String(user.PwdHash);

            byte[] calcHash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);

            return hash.SequenceEqual(calcHash);
        }

        private bool ValidateEmail(string email)
        {
            return new Regex(@"^(?=.{1,64}@)[A-Za-z0-9_-]+(\\.[A-Za-z0-9_-]+)*@[^-][A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$").IsMatch(email);
        }

        public async Task<AuthResponse> Register(RegisterRequest registerRequest)
        {
            if (await _userRepository.CheckIfUsernameExists(registerRequest.Username))
            {
                return new AuthResponse { IsSuccessful = false, Message = "Username already exists" };
            }

            if(await _userRepository.CheckIfEmailExists(registerRequest.Email))
            {
                return new AuthResponse { IsSuccessful = false, Message = "Email already exists" };
            }

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string b64Salt = Convert.ToBase64String(salt);

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: registerRequest.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);

            string b64Hash = Convert.ToBase64String(hash);

            await _userRepository.CreateUserAsync(new()
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                PwdSalt = b64Salt,
                PwdHash = b64Hash
            });

            await _unitOfWork.SaveAsync();

            return new AuthResponse { IsSuccessful = true, Message = "Registration successful" };
        }
    }
}
