import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { CreateRecipeRequest } from '../../../models/recipe.models';
import { Ingredient } from '../../../models/ingredient.model';

@Component({
  selector: 'app-recipe-create',
  templateUrl: './recipe-create.component.html',
  styleUrls: ['./recipe-create.component.scss']
})
export class RecipeCreateComponent implements OnInit {
  recipeForm: FormGroup = this.initForm();
  submitted = false;
  loading = false;
  availableIngredients: Ingredient[] = [];
  fieldInstructions: any;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {
    this.initFieldInstructions();
  }

  ngOnInit(): void {
    this.loadIngredients();
  }

  private initFieldInstructions(): void {
    this.fieldInstructions = {
      name: this.translate.instant('RECIPES.FORM.HELP_TEXTS.NAME'),
      description: this.translate.instant('RECIPES.FORM.HELP_TEXTS.DESCRIPTION'),
      preparationTime: this.translate.instant('RECIPES.FORM.HELP_TEXTS.PREPARATION_TIME'),
      cookingTime: this.translate.instant('RECIPES.FORM.HELP_TEXTS.COOKING_TIME'),
      servings: this.translate.instant('RECIPES.FORM.HELP_TEXTS.SERVINGS'),
      difficulty: this.translate.instant('RECIPES.FORM.HELP_TEXTS.DIFFICULTY'),
      instructions: this.translate.instant('RECIPES.FORM.HELP_TEXTS.INSTRUCTIONS'),
      totalCost: this.translate.instant('RECIPES.FORM.HELP_TEXTS.TOTAL_COST'),
      yieldPerPortion: this.translate.instant('RECIPES.FORM.HELP_TEXTS.YIELD_PER_PORTION'),
      ingredients: this.translate.instant('RECIPES.FORM.HELP_TEXTS.INGREDIENTS'),
      recipeIngredients: this.translate.instant('RECIPES.FORM.HELP_TEXTS.RECIPE_INGREDIENTS')
    };
  }

  get f() { return this.recipeForm.controls; }
  get ingredients() { return this.f['ingredients'] as FormArray; }

  private initForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      difficulty: ['', [Validators.required]],
      preparationTime: ['', [Validators.required, Validators.min(1), Validators.max(999)]],
      cookingTime: ['', [Validators.required, Validators.min(0), Validators.max(999)]],
      servings: ['', [Validators.required, Validators.min(1), Validators.max(99)]],
      instructions: ['', [Validators.required, Validators.minLength(20), Validators.maxLength(2000)]],
      totalCost: ['', [Validators.required, Validators.min(0.01), Validators.max(9999.99)]],
      yieldPerPortion: ['', [Validators.required, Validators.min(0.01), Validators.max(999.99)]],
      ingredients: this.fb.array([])
    });
  }

  private loadIngredients(): void {
    this.loading = true;
    this.ingredientService.getAll().subscribe({
      next: (ingredients) => {
        this.availableIngredients = ingredients;
        this.loading = false;
        if (this.ingredients.length === 0) {
          this.addIngredient();
        }
      },
      error: (error) => {
        console.error('Error loading ingredients:', error);
        this.toastr.error(
          this.translate.instant('RECIPES.MESSAGES.LOAD_INGREDIENTS_ERROR'),
          this.translate.instant('RECIPES.MESSAGES.ERROR')
        );
        this.loading = false;
      }
    });
  }

  addIngredient(): void {
    const ingredientForm = this.fb.group({
      ingredientId: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(0.01)]]
    });
    this.ingredients.push(ingredientForm);
  }

  removeIngredient(index: number): void {
    if (this.ingredients.length > 1) {
      this.ingredients.removeAt(index);
    } else {
      this.toastr.warning(
        this.translate.instant('RECIPES.MESSAGES.RECIPE_MUST_HAVE_AT_LEAST_ONE_INGREDIENT')
      );
    }
  }

  getIngredientName(ingredientId: string): string {
    const ingredient = this.availableIngredients.find(i => i.id === ingredientId);
    return ingredient ? ingredient.name : '';
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.recipeForm.valid) {
      this.loading = true;
      const formValue = this.recipeForm.value;
      
      const recipe: CreateRecipeRequest = {
        name: formValue.name,
        description: formValue.description,
        difficulty: formValue.difficulty,
        preparationTime: formValue.preparationTime,
        cookingTime: formValue.cookingTime,
        servings: formValue.servings,
        instructions: formValue.instructions,
        totalCost: formValue.totalCost,
        yieldPerPortion: formValue.yieldPerPortion,
        ingredients: formValue.ingredients.map((ing: any) => ({
          ingredientId: ing.ingredientId,
          ingredientName: this.getIngredientName(ing.ingredientId),
          quantity: ing.quantity
        }))
      };

      this.recipeService.create(recipe).subscribe({
        next: () => {
          this.toastr.success(
            this.translate.instant('RECIPES.MESSAGES.CREATE_SUCCESS'),
            this.translate.instant('RECIPES.MESSAGES.SUCCESS')
          );
          this.router.navigate(['/recipes']);
        },
        error: (error) => {
          console.error('Error creating recipe:', error);
          this.toastr.error(
            this.translate.instant('RECIPES.MESSAGES.CREATE_ERROR'),
            this.translate.instant('RECIPES.MESSAGES.ERROR')
          );
        },
        complete: () => {
          this.loading = false;
        }
      });
    } else {
      this.toastr.error(
        this.translate.instant('RECIPES.MESSAGES.PLEASE_CORRECT_ERRORS')
      );
    }
  }

  calculateYieldPerPortion(): void {
    const totalCost = this.f['totalCost'].value;
    const servings = this.f['servings'].value;
    
    if (totalCost && servings && servings > 0) {
      const yieldPerPortion = totalCost / servings;
      this.f['yieldPerPortion'].setValue(yieldPerPortion.toFixed(2));
    }
  }

  onCancel(): void {
    this.router.navigate(['/recipes']);
  }
}
