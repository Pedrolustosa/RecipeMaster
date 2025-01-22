import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { Recipe, UpdateRecipeRequest } from '../../../models/recipe.models';
import { Ingredient } from '../../../models/ingredient.model';
import { firstValueFrom } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-recipe-edit',
  templateUrl: './recipe-edit.component.html',
  styleUrls: ['./recipe-edit.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, NgxSpinnerModule]
})
export class RecipeEditComponent implements OnInit {
  recipeForm!: FormGroup;
  recipeId!: string;
  loading = false;
  submitted = false;
  ingredientList: Ingredient[] = [];
  availableIngredients: Ingredient[] = [];

  // Field help texts
  fieldInstructions = {
    name: 'Enter a descriptive name for your recipe (3-100 characters)',
    description: 'Provide a brief overview of your recipe (10-500 characters)',
    preparationTime: 'Time needed for preparation in minutes (1-999)',
    cookingTime: 'Time needed for cooking in minutes (0-999, 0 if no cooking required)',
    servings: 'Number of portions this recipe yields (1-50)',
    difficulty: 'Select the difficulty level of preparing this recipe',
    instructions: 'Detailed step-by-step cooking instructions (20-2000 characters)',
    totalCost: 'Total cost of all ingredients in USD (0.01-9999.99)',
    yieldPerPortion: 'Cost per serving in USD (0.01-999.99)',
    ingredients: 'Select ingredients and specify quantities (at least 1 ingredient required)'
  };

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.recipeId = params['id'];
      this.initForm();
      this.loadIngredients().then(() => {
        this.loadRecipe();
      });
    });
  }

  private initForm(): void {
    this.recipeForm = this.formBuilder.group({
      id: [''],
      name: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(100)
      ]],
      description: ['', [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(500)
      ]],
      preparationTime: ['', [
        Validators.required,
        Validators.min(1),
        Validators.max(999)
      ]],
      cookingTime: ['', [
        Validators.required,
        Validators.min(0),
        Validators.max(999)
      ]],
      servings: ['', [
        Validators.required,
        Validators.min(1),
        Validators.max(50)
      ]],
      difficulty: ['', [Validators.required]],
      instructions: ['', [
        Validators.required,
        Validators.minLength(20),
        Validators.maxLength(2000)
      ]],
      totalCost: ['', [
        Validators.required,
        Validators.min(0.01),
        Validators.max(9999.99)
      ]],
      yieldPerPortion: ['', [
        Validators.required,
        Validators.min(0.01),
        Validators.max(999.99)
      ]],
      ingredients: this.formBuilder.array([], [Validators.required, Validators.minLength(1)])
    });
  }

  // Getter for easy access to form fields
  get f() { return this.recipeForm.controls; }

  // Getter for ingredients form array
  get ingredients() {
    return this.f['ingredients'] as FormArray;
  }

  addIngredient(): void {
    const ingredientForm = this.formBuilder.group({
      ingredientId: ['', [Validators.required]],
      quantity: ['', [
        Validators.required,
        Validators.min(0.01),
        Validators.max(9999.99)
      ]]
    });

    this.ingredients.push(ingredientForm);
  }

  removeIngredient(index: number): void {
    if (this.ingredients.length > 1) {
      this.ingredients.removeAt(index);
    } else {
      this.toastr.warning('Recipe must have at least one ingredient.');
    }
  }

  private async loadIngredients(): Promise<void> {
    try {
      this.spinner.show();
      const ingredients = await firstValueFrom(this.ingredientService.getAll());
      this.ingredientList = ingredients;
      this.availableIngredients = [...ingredients];
    } catch (error) {
      this.toastr.error('Failed to load ingredients');
      console.error('Error loading ingredients:', error);
    } finally {
      this.spinner.hide();
    }
  }

  private async loadRecipe(): Promise<void> {
    try {
      this.loading = true;
      const recipe = await firstValueFrom(this.recipeService.getById(this.recipeId));
      
      // Clear existing ingredients
      while (this.ingredients.length) {
        this.ingredients.removeAt(0);
      }

      // Add each ingredient from the recipe
      recipe.ingredients.forEach(ingredient => {
        const ingredientForm = this.formBuilder.group({
          ingredientId: [ingredient.ingredientId, [Validators.required]],
          quantity: [ingredient.quantity, [
            Validators.required,
            Validators.min(0.01),
            Validators.max(9999.99)
          ]]
        });
        this.ingredients.push(ingredientForm);
      });

      // Update form with recipe data
      this.recipeForm.patchValue({
        name: recipe.name,
        description: recipe.description,
        preparationTime: recipe.preparationTime,
        cookingTime: recipe.cookingTime,
        servings: recipe.servings,
        difficulty: recipe.difficulty,
        instructions: recipe.instructions,
        totalCost: recipe.totalCost,
        yieldPerPortion: recipe.yieldPerPortion
      });

    } catch (error) {
      this.toastr.error('Failed to load recipe. Please try again.');
      this.router.navigate(['/recipes']);
    } finally {
      this.loading = false;
    }
  }

  onIngredientChange(index: number, event: Event): void {
    const select = event.target as HTMLSelectElement;
    const ingredientId = select.value;
    const ingredient = this.availableIngredients.find(i => i.id === ingredientId);
    
    if (ingredient) {
      const ingredientForm = this.ingredients.at(index);
      ingredientForm.patchValue({ ingredientId: ingredient.id });
    }
  }

  getValidationMessage(fieldName: string): string {
    const control = this.f[fieldName];
    if (!control || !control.errors || !control.touched) return '';

    const errors = control.errors;
    if (errors['required']) return `${fieldName} is required`;
    if (errors['minlength']) return `${fieldName} must be at least ${errors['minlength'].requiredLength} characters`;
    if (errors['maxlength']) return `${fieldName} cannot exceed ${errors['maxlength'].requiredLength} characters`;
    if (errors['min']) return `${fieldName} must be greater than ${errors['min'].min}`;
    if (errors['max']) return `${fieldName} must be less than ${errors['max'].max}`;
    if (errors['email']) return `Invalid email format`;

    return 'Invalid field';
  }

  async onSubmit(): Promise<void> {
    this.submitted = true;

    if (this.recipeForm.invalid) {
      this.toastr.error('Please correct the errors in the form before submitting.');
      return;
    }

    try {
      this.loading = true;
      const formValue = this.recipeForm.value;
      
      const recipe: UpdateRecipeRequest = {
        id: formValue.id,
        name: formValue.name,
        description: formValue.description,
        preparationTime: formValue.preparationTime,
        cookingTime: formValue.cookingTime,
        servings: formValue.servings,
        difficulty: formValue.difficulty,
        instructions: formValue.instructions,
        totalCost: formValue.totalCost,
        yieldPerPortion: formValue.yieldPerPortion,
        ingredients: formValue.ingredients.map((ing: any) => ({
          ingredientId: ing.ingredientId,
          quantity: ing.quantity
        }))
      };

      await firstValueFrom(this.recipeService.update(this.recipeId, recipe));
      this.toastr.success('Recipe updated successfully!');
      this.router.navigate(['/recipes']);
    } catch (error) {
      this.toastr.error('Failed to update recipe. Please try again.');
    } finally {
      this.loading = false;
    }
  }

  // Helper method to calculate yield per portion
  calculateYieldPerPortion(): void {
    const totalCost = this.f['totalCost'].value;
    const servings = this.f['servings'].value;
    
    if (totalCost && servings && servings > 0) {
      const yieldPerPortion = totalCost / servings;
      this.f['yieldPerPortion'].setValue(yieldPerPortion.toFixed(2));
    }
  }
}
