using AutoMapper;
using CatsApp.Api.Models;
using CatsApp.Domain.Aggregates;

namespace CatsApp.Api.Mapping;

public class CatMappingProfile : Profile
{
    public CatMappingProfile()
    {
        CreateMap<Cat, CatModel>().ReverseMap()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age)); 
    }
}
