namespace ServiceLayer.Provider
{
    public interface IJwtTokenProvider
    {
        string GenerateToken(string password);
    }
}
