using AutoMapper;
using PriceLoaderWebApp.Domain.Entities;
using PriceLoaderWebApp.Application.DTOs;

namespace PriceLoaderWebApp.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Маппинг из сущности в DTO
            CreateMap<PriceItem, PriceItemDto>()
                .ForMember(dest => dest.Vendor, opt => opt.MapFrom(src => src.Vendor))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));
        }
    }
}