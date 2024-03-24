using Microsoft.AspNetCore.Http;
using ServiceLayer.ServiceModel;

namespace ServiceLayer.Service.Abstraction
{
    public interface ITouristLocationsService
    {
        Task<UploadResponse> AddTouristLocationsFromXmlUsingXsdValidationAsync(IFormFile xml);
        Task<UploadResponse> AddTouristLocationsFromXmlUsingRngValidationAsync(IFormFile xml);
    }
}
