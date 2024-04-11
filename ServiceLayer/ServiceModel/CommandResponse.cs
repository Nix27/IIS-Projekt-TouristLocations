namespace ServiceLayer.ServiceModel
{
    public class CommandResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = null!;
    }
}
