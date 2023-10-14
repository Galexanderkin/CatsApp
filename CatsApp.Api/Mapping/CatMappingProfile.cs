using AutoMapper;
using CatsApp.Api.Models;
using CatsApp.Application.Commands;
using CatsApp.Application.Queries;
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

        CreateMap<CreateCatCommand, CatModel>().ReverseMap()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age));

        CreateMap<GetCatPageQuery, SearchModel>().ReverseMap()
            .ForMember(dest => dest.SearchText, opt => opt.MapFrom(src => src.SearchText))
            .ForMember(dest => dest.PageNum, opt => opt.MapFrom(src => src.PageNum))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}
