export interface Ingredient {
  id: string;
  name: string;
  description: string;
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
  description: string;
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
  description: string;
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
