namespace ServiceLayer.ServiceModel
{
    public class AuthResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = null!;
    }
}
