namespace ServiceLayer.ServiceModel
{
    public class RefreshRequest
    {
        public string ExpiredAccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
