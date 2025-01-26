using AutoMapper;
using RecipeMaster.Core.Entities;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Commands.Recipes;

namespace RecipeMaster.Application.Mappings;

public class RecipeMappingProfile : Profile
{
    public RecipeMappingProfile()
    {
        CreateMap<Recipe, RecipeDTO>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients.Select(i => new RecipeIngredientDTO
                {
                    IngredientId = i.Ingredient.Id,
                    IngredientName = i.Ingredient.Name,
                    Quantity = i.Quantity
                }).ToList()))
            .ReverseMap();

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
            .ReverseMap();

        CreateMap<UpdateRecipeIngredientDTO, RecipeIngredient>()
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredient, opt => opt.Ignore());

        CreateMap<RecipeDTO, CreateRecipeCommand>()
            .ReverseMap();
    }
}
