using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dto;
using ServiceLayer.Service.Abstraction;

namespace TouristLocationsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristLocationsController(ITouristLocationsService touristLocationsService) : ControllerBase
    {
        private readonly ITouristLocationsService _touristLocationsService = touristLocationsService;

        [Authorize]
        [HttpPost("uploadxsdvalidation")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddTouristLocationsFromXmlUsingXsdValidationAsync(IFormFile xml)
        {
            var response = await _touristLocationsService.AddTouristLocationsFromXmlUsingXsdValidationAsync(xml);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPost("uploadrngvalidation")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddTouristLocationsFromXmlUsingRngValidationAsync(IFormFile xml)
        {
            var response = await _touristLocationsService.AddTouristLocationsFromXmlUsingRngValidationAsync(xml);
            return Ok(response);
        }

        [HttpGet("getAllTouristLocations")]
        public async Task<IActionResult> GetAllTouristLocations()
        {
            var touristLocations = await _touristLocationsService.GetAllTouristLocationsAsync();
            return Ok(touristLocations);
        }

        [HttpGet("getTouristLocation/{id}")]
        public async Task<IActionResult> GetTouristLocation(int id)
        {
            var touristLocation = await _touristLocationsService.GetTouristLocationAsync(id);
            return Ok(touristLocation);
        }

        [HttpPost("addTouristLocation")]
        public async Task<IActionResult> AddTouristLocation(TouristLocationDto touristLocationDto)
        {
            var response = await _touristLocationsService.AddTouristLocationAsync(touristLocationDto);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [HttpPut("updateTouristLocation")]
        public async Task<IActionResult> UpdateTouristLocation(TouristLocationDto touristLocationDto)
        {
            var response = await _touristLocationsService.UpdateTouristLocationAsync(touristLocationDto);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("deleteTouristLocation/{id}")]
        public async Task<IActionResult> DeleteTouristLocation(int id)
        {
            var response = await _touristLocationsService.DeleteTouristLocationAsync(id);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }
    }
}
