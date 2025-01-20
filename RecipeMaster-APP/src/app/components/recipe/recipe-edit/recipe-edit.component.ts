import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { Recipe, UpdateRecipeRequest } from '../../../models/recipe.models';
import { Ingredient } from '../../../models/ingredient.model';

@Component({
  selector: 'app-recipe-edit',
  templateUrl: './recipe-edit.component.html',
  styleUrls: ['./recipe-edit.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, NgxSpinnerModule]
})
export class RecipeEditComponent implements OnInit {
  recipeId!: string;
  recipeForm!: FormGroup;
  submitted = false;
  loading = false;
  availableIngredients: Ingredient[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.loadIngredients().then(() => {
      this.route.params.subscribe(params => {
        this.recipeId = params['id'];
        if (this.recipeId) {
          this.loadRecipe();
        }
      });
    });
  }

  get f() { return this.recipeForm.controls; }
  get ingredients() { return this.f['ingredients'] as FormArray; }

  private loadIngredients(): Promise<void> {
    this.loading = true;
    return new Promise<void>((resolve, reject) => {
      this.ingredientService.getAll().subscribe({
        next: (ingredients) => {
          this.availableIngredients = ingredients;
          this.loading = false;
          resolve();
        },
        error: (error) => {
          this.toastr.error('Failed to load ingredients', 'Error');
          console.error('Error loading ingredients:', error);
          this.loading = false;
          reject(error);
        }
      });
    });
  }

  private loadRecipe(): void {
    if (!this.availableIngredients.length) {
      this.toastr.error('No ingredients available', 'Error');
      return;
    }

    this.loading = true;
    this.recipeService.getById(this.recipeId).subscribe({
      next: (recipe: Recipe) => {
        this.recipeForm.patchValue({
          name: recipe.name,
          description: recipe.description,
          preparationTime: recipe.preparationTime,
          cookingTime: recipe.cookingTime,
          servings: recipe.servings,
          difficulty: recipe.difficulty,
          instructions: recipe.instructions
        });

        while (this.ingredients.length) {
          this.ingredients.removeAt(0);
        }

        recipe.ingredients.forEach(ingredient => {
          this.addIngredient(ingredient);
        });

        this.loading = false;
      },
      error: (error) => {
        this.toastr.error('Failed to load recipe', 'Error');
        console.error('Error loading recipe:', error);
        this.loading = false;
        this.router.navigate(['/recipes']);
      }
    });
  }

  initForm(): void {
    this.recipeForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      preparationTime: ['', [Validators.required, Validators.min(1)]],
      cookingTime: ['', [Validators.required, Validators.min(0)]],
      servings: ['', [Validators.required, Validators.min(1)]],
      difficulty: ['', [Validators.required]],
      instructions: ['', [Validators.required, Validators.minLength(20)]],
      ingredients: this.formBuilder.array([])
    });
  }

  addIngredient(ingredient?: any): void {
    this.ingredients.push(
      this.formBuilder.group({
        ingredientId: [ingredient ? ingredient.ingredientId : '', Validators.required],
        quantity: [ingredient ? ingredient.quantity : '', [Validators.required, Validators.min(1)]]
      })
    );
  }

  removeIngredient(index: number): void {
    this.ingredients.removeAt(index);
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.recipeForm.invalid) {
      return;
    }

    this.loading = true;

    const request: UpdateRecipeRequest = {
      name: this.recipeForm.value.name,
      description: this.recipeForm.value.description,
      preparationTime: this.recipeForm.value.preparationTime,
      cookingTime: this.recipeForm.value.cookingTime,
      servings: this.recipeForm.value.servings,
      difficulty: this.recipeForm.value.difficulty,
      instructions: this.recipeForm.value.instructions,
      ingredients: this.recipeForm.value.ingredients
    };

    this.recipeService.update(this.recipeId, request).subscribe({
      next: () => {
        this.toastr.success('Recipe updated successfully', 'Success');
        this.router.navigate(['/recipes']);
      },
      error: (error) => {
        this.toastr.error('Failed to update recipe', 'Error');
        console.error('Error updating recipe:', error);
        this.loading = false;
      }
    });
  }
}
