using AutoMapper;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Core.Entities;

namespace RecipeMaster.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Ingredient, IngredientDTO>().ReverseMap();
        CreateMap<Recipe, RecipeDTO>().ReverseMap();
        CreateMap<RecipeIngredient, RecipeIngredientDTO>().ReverseMap();
    }
}
