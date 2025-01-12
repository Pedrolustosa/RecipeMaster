using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeMaster.Application.DTOs
{
    public class UpdateRecipeIngredientDTO
    {
        public Guid IngredientId { get; set; }
        public decimal Quantity { get; set; }
    }
}
