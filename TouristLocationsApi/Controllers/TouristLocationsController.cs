using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.ServiceModel;

namespace TouristLocationsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristLocationsController(ITouristLocationsService touristLocationsService) : ControllerBase
    {
        private readonly ITouristLocationsService _touristLocationsService = touristLocationsService;

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddTouristLocationsFromXmlAsync(IFormFile xml)
        {
            var response = await _touristLocationsService.AddTouristLocationsFromXmlAsync(xml);
            return Ok(response);
        }
    }
}
