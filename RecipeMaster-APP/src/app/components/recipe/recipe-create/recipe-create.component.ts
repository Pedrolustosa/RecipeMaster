import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { CreateRecipeRequest } from '../../../models/recipe.models';
import { Ingredient } from '../../../models/ingredient.model';
import { firstValueFrom } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-recipe-create',
  templateUrl: './recipe-create.component.html',
  styleUrls: ['./recipe-create.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, NgxSpinnerModule]
})
export class RecipeCreateComponent implements OnInit {
  recipeForm!: FormGroup;
  loading = false;
  submitted = false;
  ingredientList: Ingredient[] = [];

  // Field help texts
  fieldInstructions = {
    name: 'Enter a clear and descriptive name for your recipe. E.g., "Chocolate Cake with Strawberry Frosting" (3-100 characters)',
    description: 'Briefly describe your recipe, including special features and main flavors. E.g., "A fluffy chocolate cake with fresh strawberry frosting, perfect for parties" (10-500 characters)',
    preparationTime: 'Time needed to prepare all ingredients, including cutting, measuring, and organizing (1-999 minutes)',
    cookingTime: 'Total cooking/baking time. Enter 0 if no cooking required (e.g., salads) (0-999 minutes)',
    servings: 'Number of portions this recipe yields. Consider average portion sizes per person (1-50 servings)',
    difficulty: 'Choose difficulty based on required technique: Easy (beginners), Medium (basic knowledge), Hard (experience needed), Expert (advanced techniques)',
    instructions: 'List detailed step-by-step preparation instructions. Include temperatures, times, and important tips. Separate each step on a new line (20-2000 characters)',
    totalCost: 'Approximate total cost of all ingredients in USD. Use average market prices (0.01-9999.99)',
    yieldPerPortion: 'Cost per serving in USD (total cost divided by number of servings) (0.01-999.99)',
    ingredients: 'Select required ingredients and specify exact quantities (e.g., "2 cups", "300g", "3 units"). Minimum 1 ingredient',
    recipeIngredients: 'Click "Add Ingredient" to include ingredients in your recipe. For each ingredient: 1) Select from the dropdown list, 2) Specify the exact quantity (e.g., 2, 3.5), 3) Choose the measurement unit (e.g., cups, grams, pieces). Make sure to add all necessary ingredients for your recipe.'
  };

  constructor(
    private formBuilder: FormBuilder,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.initForm();
    this.loadIngredients();
  }

  ngOnInit(): void {
    this.addIngredient();
  }

  private initForm(): void {
    this.recipeForm = this.formBuilder.group({
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
    return this.recipeForm.get('ingredients') as FormArray;
  }

  addIngredient(): void {
    const ingredientForm = this.formBuilder.group({
      ingredientId: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(0.01)]]
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
      this.ingredientList = await firstValueFrom(this.ingredientService.getAll());
    } catch (error) {
      this.toastr.error('Failed to load ingredients');
      console.error('Error loading ingredients:', error);
    } finally {
      this.spinner.hide();
    }
  }

  getIngredientName(ingredientId: string): string {
    const ingredient = this.ingredientList.find(i => i.id === ingredientId);
    return ingredient ? ingredient.name : '';
  }

  onIngredientChange(index: number, event: Event): void {
    const select = event.target as HTMLSelectElement;
    const ingredientId = select.value;
    const ingredient = this.ingredientList.find(i => i.id === ingredientId);
    
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
      
      const recipe: CreateRecipeRequest = {
        name: formValue.name,
        description: formValue.description,
        preparationTime: formValue.preparationTime,
        cookingTime: formValue.cookingTime,
        servings: formValue.servings,
        difficulty: formValue.difficulty,
        instructions: formValue.instructions,
        totalCost: formValue.totalCost,
        yieldPerPortion: formValue.yieldPerPortion,
        ingredients: formValue.ingredients.map((ing: any) => {
          const ingredient = this.ingredientList.find(i => i.id === ing.ingredientId);
          return {
            ingredientId: ing.ingredientId,
            ingredientName: ingredient ? ingredient.name : '',
            quantity: ing.quantity
          };
        })
      };

      await firstValueFrom(this.recipeService.create(recipe));
      this.toastr.success('Recipe created successfully!');
      this.router.navigate(['/recipes']);
    } catch (error) {
      this.toastr.error('Failed to create recipe. Please try again.');
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
