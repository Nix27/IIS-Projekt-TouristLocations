using CookComputing.XmlRpc;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Abstraction;

namespace TouristLocationsApi.Controllers
{
    [Route("api/[controller]/xmlrpc")]
    [ApiController]
    public class CityTemperaturesController : ControllerBase
    {
        [HttpPost("getTemperatureOfCity")]
        public async Task<IActionResult> GetTemperatureOfCity(string cityName)
        {
            try
            {
                var xmlRpcService = XmlRpcProxyGen.Create<ICityTemperaturesXmlRpcService>();
                return Ok(await xmlRpcService.GetTemperatureOfCity(cityName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
