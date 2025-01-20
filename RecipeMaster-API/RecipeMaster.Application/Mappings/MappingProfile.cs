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
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => new IngredientCost(src.Cost)))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.MinimumStockLevel, opt => opt.MapFrom(src => src.MinimumStockLevel))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => src.IsPerishable))
            .ForMember(dest => dest.OriginCountry, opt => opt.MapFrom(src => src.OriginCountry))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => src.StorageInstructions))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        CreateMap<Ingredient, IngredientCostDTO>()
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost.Value))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => src.IsPerishable))
            .ForMember(dest => dest.OriginCountry, opt => opt.MapFrom(src => src.OriginCountry));
        CreateMap<Ingredient, IngredientUsageDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<Recipe, RecipeDTO>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new RecipeIngredientDTO
                {
                    IngredientId = i.Ingredient.Id,
                    IngredientName = i.Ingredient.Name,
                    Quantity = i.Quantity
                }).ToList() ?? new List<RecipeIngredientDTO>()))
            .ReverseMap()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new RecipeIngredient(src.Id, i.IngredientId, i.Quantity)).ToList()));
        CreateMap<UpdateRecipeDTO, UpdateRecipeCommand>();
        CreateMap<UpdateRecipeCommand, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new RecipeIngredient(src.Id, i.IngredientId, i.Quantity))));

        CreateMap<RecipeIngredient, RecipeIngredientDTO>()
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();
        CreateMap<UpdateRecipeIngredientDTO, RecipeIngredient>()
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredient, opt => opt.Ignore());
    }
}
