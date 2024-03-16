using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;

namespace ServiceLayer.Provider
{
    internal class JwtTokenProvider(IConfiguration configuration) : IJwtTokenProvider
    {
        public string GenerateToken(string password)
        {
            throw new NotImplementedException();
        }
    }
}
