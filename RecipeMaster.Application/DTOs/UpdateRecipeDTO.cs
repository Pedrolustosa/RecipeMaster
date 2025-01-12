using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeMaster.Application.DTOs
{
    public class UpdateRecipeDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UpdateRecipeIngredientDTO> Ingredients { get; set; }
    }
}
