export interface Recipe {
    id: string;
    name: string;
    description: string;
    preparationTime: number;
    cookingTime: number;
    servings: number;
    difficulty: string;
    instructions: string;
    totalCost: number;
    yieldPerPortion: number;
    ingredients: RecipeIngredient[];
}

export interface RecipeIngredient {
    ingredientId: string;
    ingredientName: string;
    quantity: number;
}

export interface CreateRecipeRequest {
    name: string;
    description: string;
    preparationTime: number;
    cookingTime: number;
    servings: number;
    difficulty: string;
    instructions: string;
    totalCost: number;
    yieldPerPortion: number;
    ingredients: CreateRecipeIngredient[];
}

export interface CreateRecipeIngredient {
    ingredientId: string;
    ingredientName: string;
    quantity: number;
}

export interface UpdateRecipeRequest {
    id: string;
    name: string;
    description: string;
    preparationTime: number;
    cookingTime: number;
    servings: number;
    difficulty: string;
    instructions: string;
    totalCost: number;
    yieldPerPortion: number;
    ingredients: {
        ingredientId: string;
        quantity: number;
    }[];
}

export interface UpdateRecipeIngredient {
    ingredientId: string;
    quantity: number;
}
