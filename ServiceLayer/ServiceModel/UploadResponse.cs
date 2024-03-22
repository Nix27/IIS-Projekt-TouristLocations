namespace ServiceLayer.ServiceModel
{
    public class UploadResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = null!;
    }
}
