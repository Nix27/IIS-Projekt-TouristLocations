using CookComputing.XmlRpc;

namespace ServiceLayer.Service.Abstraction
{
    [XmlRpcUrl("https://localhost:44391")]
    public interface ICityTemperaturesXmlRpcService
    {
        [XmlRpcMethod("getTemperatureOfCity")]
        Task<double> GetTemperatureOfCity(string cityName);
    }
}
