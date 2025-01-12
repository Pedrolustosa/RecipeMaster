using AutoMapper;
using RecipeMaster.Application.Commands.Ingredients;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Ingredient, IngredientDTO>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.ToString()))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost.Value))
            .ReverseMap()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => Enum.Parse<MeasurementUnit>(src.Unit)))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => new IngredientCost(src.Cost)));
        CreateMap<UpdateIngredientCommand, Ingredient>()
                    .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => Enum.Parse<MeasurementUnit>(src.Unit)))
                    .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => new IngredientCost(src.Cost)));
        CreateMap<Recipe, RecipeDTO>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ReverseMap();
        CreateMap<UpdateRecipeDTO, UpdateRecipeCommand>();
        CreateMap<RecipeIngredient, RecipeIngredientDTO>()
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();
        CreateMap<UpdateRecipeIngredientDTO, RecipeIngredient>()
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredient, opt => opt.Ignore());
        CreateMap<UpdateRecipeCommand, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new RecipeIngredient(src.Id, i.IngredientId, i.Quantity))));
    }
}
