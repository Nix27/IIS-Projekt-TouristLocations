using ServiceLayer.ServiceModel;

namespace ServiceLayer.Provider
{
    internal interface IJwtTokenProvider
    {
        Tokens GenerateTokens(JwtTokenBodyInfo bodyInfo);
    }
}
