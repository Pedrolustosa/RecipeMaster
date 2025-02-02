export interface Recipe {
    id: string;
    recipeName: string;
    quantity: number;
    unitCost: number;
    quantityPerProduction: number;
    productionCost: number; 
    ingredients: RecipeIngredient[];
}

export interface RecipeIngredient {
    ingredientId: string;
    ingredientName: string;
    quantity: number;
}

export interface CreateRecipeRequest {
    recipeName: string;
    quantity: number;
    unitCost: number;
    quantityPerProduction: number;
    productionCost: number;
    ingredients: CreateRecipeIngredient[];
}

export interface CreateRecipeIngredient {
    ingredientId: string;
    ingredientName?: string;
    quantity: number;
}

export interface UpdateRecipeRequest {
    id: string;
    recipeName: string;
    quantity: number;
    unitCost: number;
    quantityPerProduction: number;
    productionCost: number;
    ingredients: {
        ingredientId: string;
        quantity: number;
    }[];
}

export interface UpdateRecipeIngredient {
    ingredientId: string;
    quantity: number;
}
