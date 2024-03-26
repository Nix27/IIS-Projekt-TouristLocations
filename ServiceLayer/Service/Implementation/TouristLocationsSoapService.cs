using AutoMapper;
using DAL.Model;
using DAL.Repository.Abstraction;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.ServiceModel;

namespace ServiceLayer.Service.Implementation
{
    internal class TouristLocationsSoapService(IRepository<Planet> planetRepository, IMapper mapper) : ITouristLocationsSoapService
    {
        private readonly IRepository<Planet> _planetRepository = planetRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<string> GetSearchedEntities(string term)
        {
            var planets = await _planetRepository.GetAllAsync(includeProperties: "Continents.Countries.Cities.TouristLocations");
            
            var touristLocationsForXml = new Planets();

            foreach (var planet in planets)
            {
                var xmlPlanet = _mapper.Map<XmlPlanet>(planet);

                foreach (var continent in planet.Continents)
                {
                    foreach (var country in continent.Countries)
                    {
                        foreach (var city in country.Cities)
                        {
                            foreach (var touristLocation in city.TouristLocations)
                            {
                                if (touristLocation.Name.Contains(term))
                                {
                                    return $"this is my response for test, term: {term}";
                                }
                            }
                        }
                    }
                }
            }
            
            return $"this is my response for test, term: {term}";
        }
    }
}
