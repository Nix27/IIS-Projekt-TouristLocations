namespace ServiceLayer.ServiceModel
{
    public class LoginResponse : AuthResponse
    {
        public Tokens Tokens { get; set; } = null!;
    }
}
