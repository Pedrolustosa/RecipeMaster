using AutoMapper;
using RecipeMaster.Core.Entities;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Core.ValueObjects;
using RecipeMaster.Application.Commands.Ingredients;

namespace RecipeMaster.Application.Mappings;

public class IngredientMappingProfile : Profile
{
    public IngredientMappingProfile()
    {
        CreateMap<Ingredient, IngredientDTO>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.ToString()))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost.Value))
            .ReverseMap()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => Enum.Parse<MeasurementUnit>(src.Unit)))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => new IngredientCost(src.Cost)));

        CreateMap<UpdateIngredientCommand, Ingredient>()
            .ReverseMap();

        CreateMap<IngredientDTO, CreateIngredientCommand>()
            .ReverseMap();

        CreateMap<Ingredient, IngredientCostDTO>()
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost.Value));

        CreateMap<Ingredient, IngredientUsageDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
