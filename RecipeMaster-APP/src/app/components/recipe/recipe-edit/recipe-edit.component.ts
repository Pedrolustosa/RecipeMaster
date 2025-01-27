import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { UpdateRecipeRequest } from '../../../models/recipe.models';
import { Ingredient } from '../../../models/ingredient.model';
import { firstValueFrom } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-recipe-edit',
  templateUrl: './recipe-edit.component.html',
  styleUrls: ['./recipe-edit.component.scss'],
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
  fieldInstructions: any;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private translate: TranslateService
  ) {
    this.initFieldInstructions();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.recipeId = params['id'];
      this.initForm();
      this.loadIngredients().then(() => {
        this.loadRecipe();
      });
    });
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

  get f() { return this.recipeForm.controls; }
  get ingredients() { return this.f['ingredients'] as FormArray; }

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
      this.toastr.warning(
        this.translate.instant('RECIPES.MESSAGES.RECIPE_MUST_HAVE_AT_LEAST_ONE_INGREDIENT')
      );
    }
  }

  private async loadIngredients(): Promise<void> {
    try {
      this.spinner.show();
      const ingredients = await firstValueFrom(this.ingredientService.getAll());
      this.ingredientList = ingredients;
      this.availableIngredients = ingredients;
    } catch (error) {
      this.toastr.error(
        this.translate.instant('RECIPES.MESSAGES.LOAD_INGREDIENTS_ERROR'),
        this.translate.instant('RECIPES.MESSAGES.ERROR')
      );
      console.error('Error loading ingredients:', error);
    } finally {
      this.spinner.hide();
    }
  }

  private async loadRecipe(): Promise<void> {
    try {
      this.loading = true;
      const recipe = await firstValueFrom(this.recipeService.getById(this.recipeId));
      
      while (this.ingredients.length) {
        this.ingredients.removeAt(0);
      }

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
      this.toastr.error(
        this.translate.instant('RECIPES.MESSAGES.LOAD_ERROR'),
        this.translate.instant('RECIPES.MESSAGES.ERROR')
      );
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
    if (errors['required']) {
      return this.translate.instant('RECIPES.FORM.VALIDATION_MESSAGES.REQUIRED', { fieldName });
    }
    if (errors['minlength']) {
      return this.translate.instant('RECIPES.FORM.VALIDATION_MESSAGES.MIN_LENGTH', {
        fieldName,
        requiredLength: errors['minlength'].requiredLength
      });
    }
    if (errors['maxlength']) {
      return this.translate.instant('RECIPES.FORM.VALIDATION_MESSAGES.MAX_LENGTH', {
        fieldName,
        requiredLength: errors['maxlength'].requiredLength
      });
    }
    if (errors['min']) {
      return this.translate.instant('RECIPES.FORM.VALIDATION_MESSAGES.MIN', {
        fieldName,
        min: errors['min'].min
      });
    }
    if (errors['max']) {
      return this.translate.instant('RECIPES.FORM.VALIDATION_MESSAGES.MAX', {
        fieldName,
        max: errors['max'].max
      });
    }
    if (errors['email']) {
      return this.translate.instant('RECIPES.FORM.VALIDATION_MESSAGES.EMAIL');
    }

    return this.translate.instant('RECIPES.FORM.VALIDATION_MESSAGES.INVALID_FIELD');
  }

  async onSubmit(): Promise<void> {
    this.submitted = true;

    if (this.recipeForm.invalid) {
      this.toastr.error(
        this.translate.instant('RECIPES.MESSAGES.PLEASE_CORRECT_ERRORS')
      );
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
        ingredients: formValue.ingredients.map((ing: any) => {
          const ingredient = this.ingredientList.find(i => i.id === ing.ingredientId);
          return {
            ingredientId: ing.ingredientId,
            ingredientName: ingredient ? ingredient.name : '',
            quantity: ing.quantity
          };
        })
      };

      await firstValueFrom(this.recipeService.update(this.recipeId, recipe));
      this.toastr.success(
        this.translate.instant('RECIPES.MESSAGES.UPDATE_SUCCESS'),
        this.translate.instant('RECIPES.MESSAGES.SUCCESS')
      );
      this.router.navigate(['/recipes']);
    } catch (error) {
      this.toastr.error(
        this.translate.instant('RECIPES.MESSAGES.UPDATE_ERROR'),
        this.translate.instant('RECIPES.MESSAGES.ERROR')
      );
    } finally {
      this.loading = false;
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
}
