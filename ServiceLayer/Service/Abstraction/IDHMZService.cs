namespace ServiceLayer.Service.Abstraction
{
    public interface IDHMZService
    {
        Task<double> GetTemperatureOfCity(string cityName);
    }
}
