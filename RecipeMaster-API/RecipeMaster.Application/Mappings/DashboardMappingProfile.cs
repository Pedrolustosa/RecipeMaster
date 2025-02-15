using AutoMapper;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Mappings;

public class DashboardMappingProfile : Profile
{
    public DashboardMappingProfile()
    {
        CreateMap<IngredientCostDTO, IngredientDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => src.IsPerishable))
            .ForMember(dest => dest.OriginCountry, opt => opt.MapFrom(src => src.OriginCountry))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.MinimumStockLevel, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<IngredientUsageDTO, IngredientDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.MinimumStockLevel, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.OriginCountry, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
    }
}
