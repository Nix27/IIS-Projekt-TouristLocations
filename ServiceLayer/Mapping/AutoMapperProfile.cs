using AutoMapper;
using DAL.Model;
using ServiceLayer.Dto;
using ServiceLayer.ServiceModel;

namespace ServiceLayer.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ContinentDto, Continent>().ReverseMap();
            CreateMap<CountryDto, Country>().ReverseMap();
            CreateMap<CityDto, City>().ReverseMap();
            CreateMap<TouristLocationDto, TouristLocation>().ReverseMap();
        }
    }
}
