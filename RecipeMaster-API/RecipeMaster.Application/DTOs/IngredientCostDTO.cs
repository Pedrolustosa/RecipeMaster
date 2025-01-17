namespace RecipeMaster.Application.DTOs;

public class IngredientCostDTO
{
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public string SupplierName { get; set; }
    public bool IsPerishable { get; set; }
    public string OriginCountry { get; set; }
}
