using ServiceLayer.ServiceModel;

namespace ServiceLayer.Provider
{
    internal interface IJwtTokenProvider
    {
        string GenerateAccessToken(JwtTokenBodyInfo bodyInfo);
        string GenerateRefreshToken();
    }
}
