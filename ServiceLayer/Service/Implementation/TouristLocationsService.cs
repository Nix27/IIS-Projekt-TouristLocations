using AutoMapper;
using Commons.Xml.Relaxng;
using DAL.Model;
using DAL.Repository.Abstraction;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.ServiceModel;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ServiceLayer.Service.Implementation
{
    public class TouristLocationsService : ITouristLocationsService
    {
        private const string XSD_PATH = "XmlSchemas/TouristLocations.xsd";
        private const string RNG_PATH = "XmlSchemas/TouristLocations.rng";

        private readonly IRepository<Planet> _planetRepository;
        private readonly IRepository<Continent> _continentRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<TouristLocation> _touristLocationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TouristLocationsService(
            IRepository<Planet> planetRepository,
            IRepository<Continent> continentRepository,
            IRepository<Country> countryRepository,
            IRepository<City> cityRepository,
            IRepository<TouristLocation> touristLocationRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _planetRepository = planetRepository;
            _continentRepository = continentRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _touristLocationRepository = touristLocationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

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

                var serializer = new XmlSerializer(typeof(Planets));
                using var stream = xml.OpenReadStream();
                var planets = (Planets)serializer.Deserialize(stream)!;

                await AddXmlEntitiesToDb(planets);

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

        private static bool IsXmlFile(IFormFile file)
        {
            if (file.ContentType != "text/xml" && file.ContentType != "application/xml")
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extension == ".xml";
        }

        private async Task AddXmlEntitiesToDb(Planets planets)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var xmlPlanet in planets.ListOfPlanets)
            {
                var planet = _mapper.Map<Planet>(xmlPlanet);
                await _planetRepository.AddAsync(planet);
                await _unitOfWork.SaveAsync();

                foreach (var xmlContinent in xmlPlanet.Continents.ListOfContinents)
                {
                    var continent = _mapper.Map<Continent>(xmlContinent);
                    continent.PlanetId = planet.Id;
                    await _continentRepository.AddAsync(continent);
                    await _unitOfWork.SaveAsync();

                    foreach (var xmlCountry in xmlContinent.Countries.ListOfCountries)
                    {
                        var country = _mapper.Map<Country>(xmlCountry);
                        country.ContinentId = continent.Id;
                        await _countryRepository.AddAsync(country);
                        await _unitOfWork.SaveAsync();

                        foreach (var xmlCity in xmlCountry.Cities.ListOfCities)
                        {
                            var city = _mapper.Map<City>(xmlCity);
                            city.CountryId = country.Id;
                            await _cityRepository.AddAsync(city);
                            await _unitOfWork.SaveAsync();

                            foreach (var xmlTouristLocation in xmlCity.TouristLocations.ListOfTouristLocations)
                            {
                                var touristLocation = _mapper.Map<TouristLocation>(xmlTouristLocation);
                                touristLocation.CityId = city.Id;
                                await _touristLocationRepository.AddAsync(touristLocation);
                                await _unitOfWork.SaveAsync();
                            }
                        }
                    }
                }
            }

            transaction.Complete();
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

                reader.InvalidNodeFound += (source, message) => throw new XmlSchemaValidationException(message);

                var serializer = new XmlSerializer(typeof(Planets));
                using var stream = xml.OpenReadStream();
                var planets = (Planets)serializer.Deserialize(stream)!;

                await AddXmlEntitiesToDb(planets);

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

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if(e.Message != null) throw new XmlSchemaValidationException(e.Message);
        }
    }
}
