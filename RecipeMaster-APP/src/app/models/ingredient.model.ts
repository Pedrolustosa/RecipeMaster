export interface Ingredient {
  id: string;
  name: string;
  unit: string;
  cost: number;
  stockQuantity: number;
  minimumStockLevel: number;
  supplierName: string;
  isPerishable: boolean;
  originCountry: string;
  storageInstructions: string;
  isActive: boolean;
}

export interface CreateIngredientCommand {
  name: string;
  unit: string;
  cost: number;
  stockQuantity: number;
  minimumStockLevel: number;
  supplierName: string;
  isPerishable: boolean;
  originCountry: string;
  storageInstructions: string;
  isActive: boolean;
}

export interface UpdateIngredientDTO {
  name: string;
  unit: string;
  cost: number;
  stockQuantity: number;
  minimumStockLevel: number;
  supplierName: string;
  isPerishable: boolean;
  originCountry: string;
  storageInstructions: string;
  isActive: boolean;
}
