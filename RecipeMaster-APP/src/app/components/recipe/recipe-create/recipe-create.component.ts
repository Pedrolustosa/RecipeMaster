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
  availableIngredients: Ingredient[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.addIngredient();
    this.loadIngredients();
  }

  private initForm(): void {
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

  addIngredient(): void {
    const ingredientForm = this.formBuilder.group({
      ingredientId: ['', Validators.required],
      ingredientName: [''],  
      quantity: ['', [Validators.required, Validators.min(1)]]
    });

    this.ingredients.push(ingredientForm);
  }

  onIngredientChange(index: number, ingredientId: string): void {
    const selectedIngredient = this.availableIngredients.find(i => i.id === ingredientId);
    if (selectedIngredient) {
      const ingredientForm = this.ingredients.at(index);
      ingredientForm.patchValue({
        ingredientName: selectedIngredient.name
      });
    }
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

    const formValue = this.recipeForm.value;
    const request: CreateRecipeRequest = {
      name: formValue.name,
      description: formValue.description,
      preparationTime: formValue.preparationTime,
      cookingTime: formValue.cookingTime,
      servings: formValue.servings,
      difficulty: formValue.difficulty,
      instructions: formValue.instructions,
      ingredients: formValue.ingredients.map((ingredient: any) => ({
        ingredientId: ingredient.ingredientId,
        ingredientName: ingredient.ingredientName,
        quantity: ingredient.quantity
      }))
    };

    this.loading = true;
    this.recipeService.create(request).subscribe({
      next: () => {
        this.toastr.success('Recipe created successfully', 'Success');
        this.router.navigate(['/recipes']);
      },
      error: (error) => {
        this.toastr.error('Failed to create recipe', 'Error');
        console.error('Error creating recipe:', error);
        this.loading = false;
      }
    });
  }
}
