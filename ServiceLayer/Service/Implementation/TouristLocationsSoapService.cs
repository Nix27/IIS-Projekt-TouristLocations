﻿using AutoMapper;
using DAL.Model;
using DAL.Repository.Abstraction;
using ServiceLayer.Dto;
using ServiceLayer.Service.Abstraction;

namespace ServiceLayer.Service.Implementation
{
    internal class TouristLocationsSoapService(IRepository<TouristLocation> touristLocationRepository, IMapper mapper) : ITouristLocationsSoapService
    {
        private readonly IRepository<TouristLocation> _touristLocationRepository = touristLocationRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<string> GetSearchedEntities(string term)
        {
            var planets = await _touristLocationRepository.GetAllAsync(includeProperties: "Continents.Countries.Cities.TouristLocations");
            
            var touristLocationsForXml = new TouristLocations();


            
            return $"this is my response for test, term: {term}";
        }
    }
}
