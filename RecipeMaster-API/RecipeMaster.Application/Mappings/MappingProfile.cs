using AutoMapper;
using RecipeMaster.Core.Entities;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Core.ValueObjects;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Application.Commands.Ingredients;

namespace RecipeMaster.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Ingredient, IngredientDTO>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.ToString()))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost.Value))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.MinimumStockLevel, opt => opt.MapFrom(src => src.MinimumStockLevel))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => src.IsPerishable))
            .ForMember(dest => dest.OriginCountry, opt => opt.MapFrom(src => src.OriginCountry))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => src.StorageInstructions))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
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

        CreateMap<IngredientDTO, CreateIngredientCommand>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.MinimumStockLevel, opt => opt.MapFrom(src => src.MinimumStockLevel))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => src.IsPerishable))
            .ForMember(dest => dest.OriginCountry, opt => opt.MapFrom(src => src.OriginCountry))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => src.StorageInstructions))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<UpdateIngredientDTO, UpdateIngredientCommand>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.MinimumStockLevel, opt => opt.MapFrom(src => src.MinimumStockLevel))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => src.IsPerishable))
            .ForMember(dest => dest.OriginCountry, opt => opt.MapFrom(src => src.OriginCountry))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => src.StorageInstructions))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<Recipe, RecipeDTO>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new RecipeIngredientDTO
                {
                    IngredientId = i.Ingredient.Id,
                    IngredientName = i.Ingredient.Name,
                    Quantity = i.Quantity
                }).ToList()))
            .ReverseMap()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new RecipeIngredient(src.Id, i.IngredientId, i.Quantity)).ToList()));

        CreateMap<UpdateRecipeDTO, UpdateRecipeCommand>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new UpdateRecipeCommand.IngredientDto
                {
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity
                }).ToList()));

        CreateMap<UpdateRecipeCommand, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new RecipeIngredient(src.Id, i.IngredientId, i.Quantity)).ToList()));

        CreateMap<RecipeIngredient, RecipeIngredientDTO>()
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();

        CreateMap<UpdateRecipeIngredientDTO, RecipeIngredient>()
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredient, opt => opt.Ignore());

        CreateMap<RecipeDTO, CreateRecipeCommand>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.PreparationTime, opt => opt.MapFrom(src => src.PreparationTime))
            .ForMember(dest => dest.CookingTime, opt => opt.MapFrom(src => src.CookingTime))
            .ForMember(dest => dest.Servings, opt => opt.MapFrom(src => src.Servings))
            .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost))
            .ForMember(dest => dest.YieldPerPortion, opt => opt.MapFrom(src => src.YieldPerPortion))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));
    }
}
