using DAL.Repository.Abstraction;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.ServiceModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ServiceLayer.Service.Implementation
{
    public class TouristLocationsService : ITouristLocationsService
    {
        private const string XSD_PATH = "XsdFile/TouristLocations.xsd";

        private readonly IRepository<DAL.Model.Planet> _planetRepository;
        private readonly IRepository<DAL.Model.Continent> _continentRepository;
        private readonly IRepository<DAL.Model.Country> _countryRepository;
        private readonly IRepository<DAL.Model.City> _cityRepository;
        private readonly IRepository<DAL.Model.TouristLocation> _touristLocationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TouristLocationsService(
            IRepository<DAL.Model.Planet> planetRepository,
            IRepository<DAL.Model.Continent> continentRepository,
            IRepository<DAL.Model.Country> countryRepository,
            IRepository<DAL.Model.City> cityRepository,
            IRepository<DAL.Model.TouristLocation> touristLocationRepository,
            IUnitOfWork unitOfWork)
        {
            _planetRepository = planetRepository;
            _continentRepository = continentRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _touristLocationRepository = touristLocationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UploadResponse> AddTouristLocationsFromXmlAsync(IFormFile xml)
        {
            if(!IsXmlFile(xml)) return new UploadResponse { IsSuccessful = false, Message = "Invalid file type" };

            try
            {
                var xmlReaderSettings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema
                };

                xmlReaderSettings.Schemas.Add(null, XSD_PATH);
                
                using var reader = XmlReader.Create(xml.OpenReadStream(), xmlReaderSettings);
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(reader);
                xmlDocument.Validate((sender, e) => throw new XmlSchemaValidationException(e.Message));

                var serializer = new XmlSerializer(typeof(Planets));
                using var stream = xml.OpenReadStream();
                var planets = (Planets)serializer.Deserialize(stream)!;

                foreach (var planet in planets.Planet)
                {
                    await _planetRepository.AddAsync(new DAL.Model.Planet { Name = planet.Name });
                    await _unitOfWork.SaveAsync();

                }

                return new UploadResponse { IsSuccessful = true, Message = "Tourist locations added successfully" };
            }
            catch (Exception ex)
            {
                return new UploadResponse { IsSuccessful = false, Message = $"Xml validation with xsd failed: {ex.Message}" };
            }
        }

        private static bool IsXmlFile(IFormFile file)
        {
            if (file.ContentType != "text/xml" && file.ContentType != "application/xml")
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extension == ".xml";
        }
    }
}
