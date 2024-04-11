using Microsoft.AspNetCore.Http;
using ServiceLayer.Dto;
using ServiceLayer.ServiceModel;

namespace ServiceLayer.Service.Abstraction
{
    public interface ITouristLocationsService
    {
        Task<IEnumerable<TouristLocationDto>> GetAllTouristLocationsAsync();
        Task<TouristLocationDto> GetTouristLocationAsync(int id);
        Task<CommandResponse> AddTouristLocationAsync(TouristLocationDto touristLocationDto);
        Task<CommandResponse> UpdateTouristLocationAsync(TouristLocationDto touristLocationDto);
        Task<CommandResponse> DeleteTouristLocationAsync(int id);
        Task<UploadResponse> AddTouristLocationsFromXmlUsingXsdValidationAsync(IFormFile xml);
        Task<UploadResponse> AddTouristLocationsFromXmlUsingRngValidationAsync(IFormFile xml);
    }
}
