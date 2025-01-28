import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { CreateRecipeRequest } from '../../../models/recipe.models';
import { Ingredient } from '../../../models/ingredient.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recipe-create',
  templateUrl: './recipe-create.component.html',
  styleUrls: ['./recipe-create.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule
  ]
})
export class RecipeCreateComponent implements OnInit {
  recipeForm!: FormGroup;
  submitted = false;
  loading = false;
  availableIngredients: Ingredient[] = [];
  fieldInstructions: any;
  difficultyLevels = [
    { key: 'EASY', value: 'easy' },
    { key: 'MEDIUM', value: 'medium' },
    { key: 'HARD', value: 'hard' },
    { key: 'EXPERT', value: 'expert' }
  ];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.initFieldInstructions();
    this.loadIngredients();
  }

  private initFieldInstructions(): void {
    this.fieldInstructions = {
      name: 'Digite um nome único e descritivo para sua receita (ex: "Bolo de Chocolate Cremoso", "Risoto de Funghi")',
      description: 'Descreva brevemente sua receita, incluindo suas principais características e sabores (ex: "Um bolo macio e úmido com cobertura de ganache")',
      difficulty: 'Escolha o nível que melhor representa a complexidade do preparo (Fácil, Médio ou Difícil)',
      preparationTime: 'Tempo necessário para separar e preparar todos os ingredientes antes de começar a cozinhar',
      cookingTime: 'Tempo total necessário para cozinhar ou assar a receita até o ponto ideal',
      servings: 'Quantidade de porções que esta receita rende (ex: 4 pessoas, 8 fatias)',
      totalCost: 'Custo total aproximado para preparar a receita completa',
      yieldPerPortion: 'Quantidade em gramas por porção (ex: 150g por fatia)',
      instructions: 'Descreva passo a passo como preparar a receita. Seja claro e específico, incluindo tempos, temperaturas e técnicas necessárias',
      ingredients: 'Selecione os ingredientes necessários para a receita',
      recipeIngredients: 'Especifique a quantidade de cada ingrediente em gramas'
    };
  }

  get f() { return this.recipeForm.controls; }
  get ingredients() { return this.f['ingredients'] as FormArray; }

  private initForm(): void {
    this.recipeForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      preparationTime: ['', [Validators.required, Validators.min(1), Validators.max(1440)]],
      cookingTime: ['', [Validators.required, Validators.min(1), Validators.max(1440)]],
      servings: ['', [Validators.required, Validators.min(1), Validators.max(100)]],
      difficulty: ['', [Validators.required]],
      totalCost: ['', [Validators.required, Validators.min(0)]],
      yieldPerPortion: ['', [Validators.required, Validators.min(0)]],
      instructions: ['', [Validators.required, Validators.minLength(30)]],
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
