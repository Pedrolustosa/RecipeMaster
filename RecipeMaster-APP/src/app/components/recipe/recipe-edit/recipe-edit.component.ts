import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { UpdateRecipeRequest } from '../../../models/recipe.models';
import { Ingredient } from '../../../models/ingredient.model';
import { CommonModule } from '@angular/common';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxSpinnerService } from 'ngx-spinner';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-recipe-edit',
  templateUrl: './recipe-edit.component.html',
  styleUrls: ['./recipe-edit.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    NgxSpinnerModule,
    RouterModule
  ]
})
export class RecipeEditComponent implements OnInit {
  recipeForm!: FormGroup;
  recipeId!: string;
  loading = false;
  submitted = false;
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
      recipeName: 'Enter a unique and descriptive name for your recipe (e.g., "Creamy Chocolate Cake", "Wild Mushroom Risotto")',
      quantity: 'Enter the total produced quantity (e.g., 100 units)',
      unitCost: 'Enter the cost per unit (e.g., $5.00)',
      quantityPerProduction: 'Enter the quantity produced per production cycle (e.g., 20 units)',
      productionCost: 'Enter the total production cost (e.g., $500.00)',
      ingredients: 'Select the ingredients required for the recipe',
      recipeIngredients: 'Specify the quantity (in grams) for each ingredient'
    };
  }

  private initForm(): void {
    this.recipeForm = this.formBuilder.group({
      id: [''],
      recipeName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      quantity: ['', [Validators.required, Validators.min(1)]],
      unitCost: ['', [Validators.required, Validators.min(0.01)]],
      quantityPerProduction: ['', [Validators.required, Validators.min(1)]],
      productionCost: ['', [Validators.required, Validators.min(0.01)]],
      ingredients: this.formBuilder.array([], [Validators.required, Validators.minLength(1)])
    });

    this.ingredients.valueChanges.subscribe(() => {
      if (this.ingredients.length === 0) {
        this.ingredients.setErrors({ required: true });
      } else {
        this.ingredients.setErrors(null);
      }
    });
  }

  get f() { return this.recipeForm.controls; }
  get ingredients() { return this.f['ingredients'] as FormArray; }

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
      this.toastr.warning(
        this.translate.instant('RECIPES.MESSAGES.RECIPE_MUST_HAVE_AT_LEAST_ONE_INGREDIENT')
      );
    }
  }

  private async loadIngredients(): Promise<void> {
    try {
      this.spinner.show();
      const ingredients = await firstValueFrom(this.ingredientService.getAll());
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

  private loadRecipe(): void {
    this.spinner.show();
    this.recipeService.getById(this.recipeId).subscribe({
      next: (recipe) => {
        this.recipeForm.patchValue({
          id: recipe.id,
          recipeName: recipe.recipeName,
          quantity: recipe.quantity,
          unitCost: recipe.unitCost,
          quantityPerProduction: recipe.quantityPerProduction,
          productionCost: recipe.productionCost
        });
        // Clear existing ingredients before adding
        (this.recipeForm.get('ingredients') as FormArray).clear();
        recipe.ingredients.forEach((ingredient) => {
          this.ingredients.push(this.createIngredientFormGroup(ingredient));
        });
        this.spinner.hide();
      },
      error: (error) => {
        this.spinner.hide();
        this.toastr.error(this.translate.instant('RECIPES.EDIT.ERRORS.LOAD_RECIPE'));
        console.error('Error loading recipe:', error);
      }
    });
  }

  createIngredientFormGroup(ingredient: any): FormGroup {
    return this.formBuilder.group({
      ingredientId: [ingredient.ingredientId, Validators.required],
      quantity: [ingredient.quantity, [Validators.required, Validators.min(0.01)]]
    });
  }

  async onSubmit(): Promise<void> {
    this.submitted = true;
    if (this.recipeForm.invalid) {
      return;
    }

    try {
      this.spinner.show();
      const formValue = this.recipeForm.value;
      const updateRequest: UpdateRecipeRequest = {
        id: this.recipeId,
        recipeName: formValue.recipeName.trim(),
        quantity: Number(formValue.quantity),
        unitCost: Number(formValue.unitCost),
        quantityPerProduction: Number(formValue.quantityPerProduction),
        productionCost: Number(formValue.productionCost),
        ingredients: formValue.ingredients
      };

      await firstValueFrom(this.recipeService.update(this.recipeId, updateRequest));
      this.toastr.success(
        this.translate.instant('RECIPES.EDIT.MESSAGES.SUCCESS'),
        this.translate.instant('COMMON.SUCCESS')
      );
      this.router.navigate(['/recipes']);
    } catch (error) {
      console.error('Error updating recipe:', error);
      this.toastr.error(
        this.translate.instant('RECIPES.EDIT.MESSAGES.SAVE_ERROR'),
        this.translate.instant('COMMON.ERROR')
      );
    } finally {
      this.spinner.hide();
    }
  }

  onCancel(): void {
    if (confirm(this.translate.instant('RECIPES.EDIT.MESSAGES.CONFIRM_CANCEL'))) {
      this.router.navigate(['/recipes']);
    }
  }
}
