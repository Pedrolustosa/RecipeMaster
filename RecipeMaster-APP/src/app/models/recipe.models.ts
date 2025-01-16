export interface Recipe {
    id: string;
    name: string;
    description: string;
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
    ingredients: CreateRecipeIngredient[];
}

export interface CreateRecipeIngredient {
    ingredientId: string;
    quantity: number;
}

export interface UpdateRecipeRequest {
    name: string;
    description: string;
    ingredients: UpdateRecipeIngredient[];
}

export interface UpdateRecipeIngredient {
    ingredientId: string;
    quantity: number;
}
