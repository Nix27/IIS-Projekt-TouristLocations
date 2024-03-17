using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.ServiceModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceLayer.Provider
{
    internal class JwtTokenProvider(IConfiguration configuration) : IJwtTokenProvider
    {
        private readonly IConfiguration _configuration = configuration;

        public Tokens GenerateTokens(JwtTokenBodyInfo bodyInfo)
        {
            var jwtKey = _configuration["Jwt:Key"]!;
            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new(JwtRegisteredClaimNames.Sub, bodyInfo.Email),
                    new(ClaimTypes.Email, bodyInfo.Email)
                }),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new(new SymmetricSecurityKey(jwtKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new()
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = ""
            };
        }
    }
}
