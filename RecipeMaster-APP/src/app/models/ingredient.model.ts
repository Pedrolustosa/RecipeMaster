export interface Ingredient {
  id: string;
  name: string;
  unit: string;
  cost: number;
}

export interface CreateIngredientCommand {
  name: string;
  unit: string;
  cost: number;
}

export interface UpdateIngredientDTO {
  name: string;
  unit: string;
  cost: number;
}
