using DAL.Model;
using DAL.Repository.Abstraction;
using ServiceLayer.Service.Abstraction;
using System.Xml.Linq;

namespace ServiceLayer.Service.Implementation
{
    internal class TouristLocationsSoapService(IRepository<Planet> planetRepository) : ITouristLocationsSoapService
    {
        private readonly IRepository<Planet> _planetRepository = planetRepository;

        public async Task<XDocument> GetXmlWithSearchedEntities(string term)
        {
            var planets = await _planetRepository.GetAllAsync(includeProperties: "Continents.Countries.Cities.TouristLocations");
            return new XDocument();
        }
    }
}
