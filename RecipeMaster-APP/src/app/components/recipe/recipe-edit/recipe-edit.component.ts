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
  ingredientList: Ingredient[] = [];
  availableIngredients: Ingredient[] = [];
  fieldInstructions: any;
  difficultyLevels = [
    { key: 'RECIPES.DIFFICULTY.EASY', value: 'easy' },
    { key: 'RECIPES.DIFFICULTY.MEDIUM', value: 'medium' },
    { key: 'RECIPES.DIFFICULTY.HARD', value: 'hard' },
    { key: 'RECIPES.DIFFICULTY.EXPERT', value: 'expert' }
  ];

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
      name: 'Digite um nome único e descritivo para sua receita (ex: "Bolo de Chocolate Cremoso", "Risoto de Funghi")',
      description: 'Descreva brevemente sua receita, incluindo suas principais características e sabores (ex: "Um bolo macio e úmido com cobertura de ganache")',
      difficulty: 'Escolha o nível que melhor representa a complexidade do preparo (Fácil, Médio ou Difícil)',
      preparationTime: 'Tempo necessário para separar e preparar todos os ingredientes antes de começar a cozinhar',
      cookingTime: 'Tempo total necessário para cozinhar ou assar a receita até o ponto ideal',
      servings: 'Quantidade de porções que esta receita rende (ex: 4 pessoas, 8 fatias)',
      instructions: 'Descreva o passo a passo completo do preparo. Seja claro e específico em cada etapa',
      totalCost: 'Custo total aproximado de todos os ingredientes utilizados na receita',
      yieldPerPortion: 'Valor sugerido por porção para venda (calculado automaticamente)',
      ingredients: 'Selecione os ingredientes necessários e especifique a quantidade de cada um',
      recipeIngredients: 'Lista dos ingredientes já adicionados à receita com suas respectivas quantidades'
    };
  }

  private initForm(): void {
    this.recipeForm = this.formBuilder.group({
      id: [''],
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      preparationTime: ['', [Validators.required, Validators.min(1), Validators.max(1440)]],
      cookingTime: ['', [Validators.required, Validators.min(1), Validators.max(1440)]],
      servings: ['', [Validators.required, Validators.min(1), Validators.max(100)]],
      difficulty: ['', [Validators.required, Validators.pattern('^(easy|medium|hard|expert)$')]],
      totalCost: ['', [Validators.required, Validators.min(0.01)]],
      yieldPerPortion: ['', [Validators.required, Validators.min(1)]],
      instructions: ['', [Validators.required, Validators.minLength(30)]],
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

  private loadRecipe(): void {
    this.spinner.show();
    this.recipeService.getById(this.recipeId).subscribe({
      next: (recipe) => {
        const difficultyLevel = this.difficultyLevels.find(level => level.value === recipe.difficulty);
        
        this.recipeForm.patchValue({
          id: recipe.id,
          name: recipe.name,
          description: recipe.description,
          preparationTime: recipe.preparationTime,
          cookingTime: recipe.cookingTime,
          servings: recipe.servings,
          difficulty: difficultyLevel?.value || '',
          totalCost: recipe.totalCost,
          yieldPerPortion: recipe.yieldPerPortion,
          instructions: recipe.instructions
        });

        recipe.ingredients.forEach(ingredient => {
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

  onIngredientChange(index: number, event: Event): void {
    const select = event.target as HTMLSelectElement;
    const ingredientId = select.value;
    const ingredient = this.availableIngredients.find(i => i.id === ingredientId);
    
    if (ingredient) {
      const ingredientForm = this.ingredients.at(index);
      ingredientForm.patchValue({ ingredientId: ingredient.id });
    }
  }

  getValidationMessage(field: string): string {
    const control = this.recipeForm.get(field);
    if (control?.errors) {
      if (control.errors['required']) {
        return this.translate.instant(`RECIPES.EDIT.VALIDATION.${field.toUpperCase()}_REQUIRED`);
      }
      if (control.errors['minlength']) {
        return this.translate.instant(`RECIPES.EDIT.VALIDATION.${field.toUpperCase()}_MIN_LENGTH`);
      }
      if (control.errors['maxlength']) {
        return this.translate.instant(`RECIPES.EDIT.VALIDATION.${field.toUpperCase()}_MAX_LENGTH`);
      }
      if (control.errors['min']) {
        return this.translate.instant(`RECIPES.EDIT.VALIDATION.${field.toUpperCase()}_MIN`);
      }
      if (control.errors['max']) {
        return this.translate.instant(`RECIPES.EDIT.VALIDATION.${field.toUpperCase()}_MAX`);
      }
      if (control.errors['pattern']) {
        return this.translate.instant(`RECIPES.EDIT.VALIDATION.${field.toUpperCase()}_PATTERN`);
      }
    }
    return '';
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
        name: formValue.name,
        description: formValue.description,
        difficulty: formValue.difficulty,
        preparationTime: formValue.preparationTime,
        cookingTime: formValue.cookingTime,
        servings: formValue.servings,
        instructions: formValue.instructions,
        totalCost: formValue.totalCost,
        yieldPerPortion: formValue.yieldPerPortion,
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

  calculateYieldPerPortion(): void {
    const totalCost = this.f['totalCost'].value;
    const servings = this.f['servings'].value;
    
    if (totalCost && servings && servings > 0) {
      const yieldPerPortion = totalCost / servings;
      this.f['yieldPerPortion'].setValue(yieldPerPortion.toFixed(2));
    }
  }
}
