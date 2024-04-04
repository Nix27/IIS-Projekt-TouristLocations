using Microsoft.Extensions.Logging;
using ServiceLayer.Service.Abstraction;
using System.Xml.Linq;

namespace ServiceLayer.Service.Implementation
{
    internal class CityTemperaturesXmlRpcService(ILogger<CityTemperaturesXmlRpcService> logger) : ICityTemperaturesXmlRpcService
    {
        private const string Url = "https://vrijeme.hr/hrvatska_n.xml";

        private readonly ILogger<CityTemperaturesXmlRpcService> _logger = logger;

        public async Task<double> GetTemperatureOfCity(string cityName)
        {
            using var client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                string xml = await response.Content.ReadAsStringAsync();

                var xDoc = XDocument.Parse(xml);

                XElement? cityElement = xDoc.Descendants("Grad").FirstOrDefault(x => x.Element("GradIme")?.Value == cityName);

                if (cityElement != null)
                {
                    string? temperature = cityElement.Element("Podatci")?.Element("Temp")?.Value;

                    if(!string.IsNullOrEmpty(temperature) && double.TryParse(temperature, out double result))
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return 0;
        }
    }
}
