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
            CreateMap<ContinentDto, Continent>();
            CreateMap<CountryDto, Country>()
                .ForMember(dest => dest.Continent, opt => opt.Ignore());
            CreateMap<CityDto, City>()
                .ForMember(dest => dest.Country, opt => opt.Ignore());
            CreateMap<TouristLocationDto, TouristLocation>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));
        }
    }
}
