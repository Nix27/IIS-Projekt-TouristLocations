using AutoMapper;
using Commons.Xml.Relaxng;
using DAL.Model;
using DAL.Repository.Abstraction;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServiceLayer.Dto;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.ServiceModel;
using System.Transactions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ServiceLayer.Service.Implementation
{
    public class TouristLocationsService(
        IRepository<Continent> continentRepository,
        IRepository<Country> countryRepository,
        IRepository<City> cityRepository,
        IRepository<TouristLocation> touristLocationRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<TouristLocationsService> logger) : ITouristLocationsService
    {
        private const string XSD_PATH = "XmlSchemas/TouristLocations.xsd";
        private const string RNG_PATH = "XmlSchemas/TouristLocations.rng";

        private readonly IRepository<Continent> _continentRepository = continentRepository;
        private readonly IRepository<Country> _countryRepository = countryRepository;
        private readonly IRepository<City> _cityRepository = cityRepository;
        private readonly IRepository<TouristLocation> _touristLocationRepository = touristLocationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<TouristLocationsService> _logger = logger;

        public async Task<UploadResponse> AddTouristLocationsFromXmlUsingXsdValidationAsync(IFormFile xml)
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

                var serializer = new XmlSerializer(typeof(TouristLocations));
                using var stream = xml.OpenReadStream();
                var touristLocations = (TouristLocations)serializer.Deserialize(stream)!;

                await AddXmlEntitiesToDb(touristLocations);

                return new UploadResponse { IsSuccessful = true, Message = "Tourist locations added successfully" };
            }
            catch (XmlSchemaValidationException ex)
            {
                return new UploadResponse { IsSuccessful = false, Message = $"Xml validation with xsd failed: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new UploadResponse { IsSuccessful = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<UploadResponse> AddTouristLocationsFromXmlUsingRngValidationAsync(IFormFile xml)
        {
            if (!IsXmlFile(xml)) return new UploadResponse { IsSuccessful = false, Message = "Invalid file type" };

            try
            {
                using XmlReader xmlReader = XmlReader.Create(xml.OpenReadStream());
                using XmlReader rngReader = new XmlTextReader(RNG_PATH);
                RelaxngPattern rngPattern = RelaxngPattern.Read(rngReader);

                using var reader = new RelaxngValidatingReader(xmlReader, rngPattern);

                while(!reader.EOF)
                {
                    reader.Read();
                }

                var serializer = new XmlSerializer(typeof(TouristLocations));
                using var stream = xml.OpenReadStream();
                var touristLocations = (TouristLocations)serializer.Deserialize(stream)!;

                await AddXmlEntitiesToDb(touristLocations);

                return new UploadResponse { IsSuccessful = true, Message = "Tourist locations added successfully" };
            }
            catch (Exception ex)
            {
                return new UploadResponse { IsSuccessful = false, Message = $"Error: {ex.Message}" };
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

        private async Task AddXmlEntitiesToDb(TouristLocations touristLocations)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var touristLocationDto in touristLocations.ListOfTouristLocations)
            {
                var continentDto = touristLocationDto.City.Country.Continent;
                Continent? newContinent;
                newContinent = await _continentRepository.GetEntityAsync(c => c.Name == continentDto.Name);
                if (newContinent == null)
                {
                    newContinent = new() { Name = continentDto.Name };
                    await _continentRepository.AddAsync(newContinent);
                    await _unitOfWork.SaveAsync();
                }

                var countryDto = touristLocationDto.City.Country;
                Country? newCountry;
                newCountry = await _countryRepository.GetEntityAsync(c => c.Name == countryDto.Name);
                if (newCountry == null)
                {
                    newCountry = new() { Name = countryDto.Name };
                    newCountry.ContinentId = newContinent.Id;
                    await _countryRepository.AddAsync(newCountry);
                    await _unitOfWork.SaveAsync();
                }

                var cityDto = touristLocationDto.City;
                City? newCity;
                newCity = await _cityRepository.GetEntityAsync(c => c.Name == cityDto.Name);
                if (newCity == null)
                {
                    newCity = new() { Name = cityDto.Name };
                    newCity.CountryId = newCountry.Id;
                    await _cityRepository.AddAsync(newCity);
                    await _unitOfWork.SaveAsync();
                }

                TouristLocation? newTouristLocation;
                newTouristLocation = await _touristLocationRepository.GetEntityAsync(tl => tl.Name == touristLocationDto.Name);
                if (newTouristLocation == null)
                {
                    newTouristLocation = new() { Name = touristLocationDto.Name, Description = touristLocationDto.Description, Rating = touristLocationDto.Rating };
                    newTouristLocation.CityId = newCity.Id;
                    await _touristLocationRepository.AddAsync(newTouristLocation);
                    await _unitOfWork.SaveAsync();
                }
            }

            transaction.Complete();
        }

        public async Task<IEnumerable<TouristLocationDto>> GetAllTouristLocationsAsync()
        {
            var allTouristLocations = await _touristLocationRepository.GetAllAsync(includeProperties: "City.Country.Continent");
            return _mapper.Map<IEnumerable<TouristLocationDto>>(allTouristLocations);
        }

        public async Task<TouristLocationDto> GetTouristLocationAsync(int id)
        {
            var touristLocation = await _touristLocationRepository.GetEntityAsync(tl => tl.Id == id, includeProperties: "City.Country.Continent");
            return _mapper.Map<TouristLocationDto>(touristLocation);
        }

        public async Task<CommandResponse> AddTouristLocationAsync(TouristLocationDto touristLocationDto)
        {
            try
            {
                await _touristLocationRepository.AddAsync(_mapper.Map<TouristLocation>(touristLocationDto));
                await _unitOfWork.SaveAsync();

                return new CommandResponse { IsSuccessful = true, Message = "Tourist location added successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding tourist location");
                return new CommandResponse { IsSuccessful = false, Message = "Error adding tourist location" };
            }
        }

        public async Task<CommandResponse> UpdateTouristLocationAsync(TouristLocationDto touristLocationDto)
        {
            try
            {
                _touristLocationRepository.Update(_mapper.Map<TouristLocation>(touristLocationDto));
                await _unitOfWork.SaveAsync();

                return new CommandResponse { IsSuccessful = true, Message = "Tourist location updated successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tourist location");
                return new CommandResponse { IsSuccessful = false, Message = "Error updating tourist location" };
            }
        }

        public async Task<CommandResponse> DeleteTouristLocationAsync(int id)
        {
            try
            {
                var touristLocation = await _touristLocationRepository.GetEntityAsync(tl => tl.Id == id);

                if (touristLocation == null)
                {
                    return new CommandResponse { IsSuccessful = false, Message = "Tourist location not found" };
                }

                _touristLocationRepository.Delete(touristLocation);
                await _unitOfWork.SaveAsync();

                return new CommandResponse { IsSuccessful = true, Message = "Tourist location deleted successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tourist location");
                return new CommandResponse { IsSuccessful = false, Message = "Error deleting tourist location" };
            }
        }
    }
}
