using AutoMapper;
using Commons.Xml.Relaxng;
using DAL.Model;
using DAL.Repository.Abstraction;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
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
        IMapper mapper) : ITouristLocationsService
    {
        private const string XSD_PATH = "XmlSchemas/TouristLocations.xsd";
        private const string RNG_PATH = "XmlSchemas/TouristLocations.rng";

        private readonly IRepository<Continent> _continentRepository = continentRepository;
        private readonly IRepository<Country> _countryRepository = countryRepository;
        private readonly IRepository<City> _cityRepository = cityRepository;
        private readonly IRepository<TouristLocation> _touristLocationRepository = touristLocationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

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
                    newContinent = _mapper.Map<Continent>(continentDto);
                    await _continentRepository.AddAsync(newContinent);
                    await _unitOfWork.SaveAsync();
                }

                var countryDto = touristLocationDto.City.Country;
                Country? newCountry;
                newCountry = await _countryRepository.GetEntityAsync(c => c.Name == countryDto.Name);
                if (newCountry == null)
                {
                    newCountry = _mapper.Map<Country>(countryDto);
                    newCountry.ContinentId = newContinent.Id;
                    await _countryRepository.AddAsync(newCountry);
                    await _unitOfWork.SaveAsync();
                }

                var cityDto = touristLocationDto.City;
                City? newCity;
                newCity = await _cityRepository.GetEntityAsync(c => c.Name == cityDto.Name);
                if (newCity == null)
                {
                    newCity = _mapper.Map<City>(cityDto);
                    newCity.CountryId = newCountry.Id;
                    await _cityRepository.AddAsync(newCity);
                    await _unitOfWork.SaveAsync();
                }

                TouristLocation? newTouristLocation;
                newTouristLocation = await _touristLocationRepository.GetEntityAsync(tl => tl.Name == touristLocationDto.Name);
                if (newTouristLocation == null)
                {
                    newTouristLocation = _mapper.Map<TouristLocation>(touristLocationDto);
                    newTouristLocation.CityId = newCity.Id;
                    await _touristLocationRepository.AddAsync(newTouristLocation);
                    await _unitOfWork.SaveAsync();
                }
            }

            transaction.Complete();
        }
    }
}
