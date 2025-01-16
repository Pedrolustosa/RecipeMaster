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
  recipeForm: FormGroup;
  recipeId: string = '';
  loading = false;
  submitted = false;
  availableIngredients: Ingredient[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.recipeForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      ingredients: this.formBuilder.array([])
    });
  }

  ngOnInit(): void {
    this.loadIngredients();
    this.route.params.subscribe(params => {
      this.recipeId = params['id'];
      this.loadRecipe();
    });
  }

  // Form getters
  get f() { return this.recipeForm.controls; }
  get ingredients() { return this.f['ingredients'] as FormArray; }

  private loadIngredients(): void {
    this.loading = true;
    this.ingredientService.getAll().subscribe({
      next: (ingredients) => {
        this.availableIngredients = ingredients;
        this.loading = false;
      },
      error: (error) => {
        this.toastr.error('Failed to load ingredients', 'Error');
        console.error('Error loading ingredients:', error);
        this.loading = false;
      }
    });
  }

  private loadRecipe(): void {
    this.loading = true;
    this.recipeService.getById(this.recipeId).subscribe({
      next: (recipe: Recipe) => {
        this.recipeForm.patchValue({
          name: recipe.name,
          description: recipe.description
        });

        // Clear existing ingredients
        while (this.ingredients.length) {
          this.ingredients.removeAt(0);
        }

        // Add recipe ingredients
        recipe.ingredients.forEach(ingredient => {
          this.ingredients.push(
            this.formBuilder.group({
              ingredientId: [ingredient.ingredientId, Validators.required],
              quantity: [ingredient.quantity, [Validators.required, Validators.min(1)]]
            })
          );
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

  addIngredient(): void {
    const ingredientForm = this.formBuilder.group({
      ingredientId: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(1)]]
    });

    this.ingredients.push(ingredientForm);
  }

  removeIngredient(index: number): void {
    if (this.ingredients.length > 1) {
      this.ingredients.removeAt(index);
    }
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.recipeForm.invalid) {
      return;
    }

    const request: UpdateRecipeRequest = {
      name: this.recipeForm.value.name,
      description: this.recipeForm.value.description,
      ingredients: this.recipeForm.value.ingredients.map((ing: any) => ({
        ingredientId: ing.ingredientId,
        quantity: ing.quantity
      }))
    };

    this.loading = true;
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
