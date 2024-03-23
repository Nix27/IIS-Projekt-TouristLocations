using AutoMapper;
using DAL.Model;
using ServiceLayer.ServiceModel;

namespace ServiceLayer.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<XmlPlanet, Planet>()
                .ForMember(dest => dest.Continents, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<XmlContinent, Continent>()
                .ForMember(dest => dest.Countries, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<XmlCountry, Country>()
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<XmlCity, City>()
                .ForMember(dest => dest.TouristLocations, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<XmlTouristLocation, TouristLocation>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ReverseMap();
        }
    }
}
