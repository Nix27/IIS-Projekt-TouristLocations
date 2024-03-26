using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.ServiceModel;

namespace TouristLocationsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristLocationsController(ITouristLocationsService touristLocationsService, ITouristLocationsSoapService touristLocationsSoapService) : ControllerBase
    {
        private readonly ITouristLocationsService _touristLocationsService = touristLocationsService;
        private readonly ITouristLocationsSoapService _touristLocationsSoapService = touristLocationsSoapService;

        [Authorize]
        [HttpPost("uploadxsdvalidation")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddTouristLocationsFromXmlUsingXsdValidationAsync(IFormFile xml)
        {
            var response = await _touristLocationsService.AddTouristLocationsFromXmlUsingXsdValidationAsync(xml);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("uploadrngvalidation")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddTouristLocationsFromXmlUsingRngValidationAsync(IFormFile xml)
        {
            var response = await _touristLocationsService.AddTouristLocationsFromXmlUsingRngValidationAsync(xml);
            return Ok(response);
        }
    }
}
