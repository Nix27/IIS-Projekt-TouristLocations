using CookComputing.XmlRpc;

namespace ServiceLayer.Service.Abstraction
{
    [XmlRpcUrl("https://localhost:8080/xmlrpc/citiestemperatures")]
    public interface ICityTemperaturesXmlRpcService
    {
        [XmlRpcMethod("getTemperatureOfCity")]
        Task<double> GetTemperatureOfCity(string cityName);
    }
}
