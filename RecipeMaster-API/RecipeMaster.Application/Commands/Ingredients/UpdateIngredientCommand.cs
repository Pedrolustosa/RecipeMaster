using MediatR;

namespace RecipeMaster.Application.Commands.Ingredients;

public class UpdateIngredientCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Unit { get; set; }
    public decimal Cost { get; set; }
    public int StockQuantity { get; set; }
    public int MinimumStockLevel { get; set; }
    public string SupplierName { get; set; }
    public bool IsPerishable { get; set; }
    public string OriginCountry { get; set; }
    public string StorageInstructions { get; set; }
    public bool IsActive { get; set; }
}
