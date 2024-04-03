using DAL.Model;
using DAL.Repository.Abstraction;
using ServiceLayer.Dto;
using ServiceLayer.Service.Abstraction;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace ServiceLayer.Service.Implementation
{
    internal class TouristLocationsSoapService(IRepository<TouristLocation> touristLocationRepository) : ITouristLocationsSoapService
    {
        private readonly IRepository<TouristLocation> _touristLocationRepository = touristLocationRepository;

        private const string XML_LOCATION = "CreatedXml/touristLocations.xml";

        public async Task<string> GetSearchedEntities(string term)
        {
            var touristLocations = await _touristLocationRepository.GetAllAsync(includeProperties: "City.Country.Continent");
            var touristLocationsForXml = ConvertToDto(touristLocations);
            CreateXmlFile(touristLocationsForXml);
            
            return SearchXmlFile(term);
        }

        private TouristLocations ConvertToDto(IEnumerable<TouristLocation> touristLocations)
        {
            var touristLocationsDto = new TouristLocations()
            {
                ListOfTouristLocations = []
            };

            foreach (var touristLocation in touristLocations)
            {
                ContinentDto continentDto = new()
                {
                    Name = touristLocation.City.Country.Continent.Name
                };

                CountryDto countryDto = new()
                {
                    Name = touristLocation.City.Country.Name,
                    Continent = continentDto
                };

                CityDto cityDto = new()
                {
                    Name = touristLocation.City.Name,
                    Country = countryDto
                };

                TouristLocationDto touristLocationDto = new()
                {
                    Name = touristLocation.Name,
                    Description = touristLocation.Description,
                    Rating = touristLocation.Rating,
                    City = cityDto
                };

                touristLocationsDto.ListOfTouristLocations.Add(touristLocationDto);
            }

            return touristLocationsDto;
        }

        private void CreateXmlFile(TouristLocations touristLocations)
        {
            var xmlSerializer = new XmlSerializer(typeof(TouristLocations));
            using var fileStream = new FileStream(XML_LOCATION, FileMode.Create);
            xmlSerializer.Serialize(fileStream, touristLocations);
        }

        private static string SearchXmlFile(string term)
        {
            XmlDocument doc = new();
            doc.Load(XML_LOCATION);

            XPathNavigator xPathNavigator = doc.CreateNavigator()!;
            XPathNodeIterator xPathNodeIterator = xPathNavigator.Select(term);

            StringBuilder sb = new();

            while (xPathNodeIterator.MoveNext())
            {
                sb.AppendLine(xPathNodeIterator.Current?.Value);
            }

            return sb.ToString();
        }
    }
}
