import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

interface Ingredient {
  id: number;
  name: string;
  unit: string;
  quantity: number;
  calories: number;
  category: string;
}

@Component({
  selector: 'app-ingredient',
  templateUrl: './ingredient.component.html',
  styleUrls: ['./ingredient.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgxSpinnerModule, FormsModule]
})
export class IngredientComponent implements OnInit {
  ingredientForm!: FormGroup;
  ingredients: Ingredient[] = [];
  isEditing = false;
  currentPage = 1;
  pageSize = 10;
  searchTerm = '';
  sortColumn = 'name';
  sortDirection = 'asc';

  measurementUnits = [
    'grams (g)',
    'kilograms (kg)',
    'milliliters (ml)',
    'liters (l)',
    'units (un)',
    'tablespoons (tbsp)',
    'teaspoons (tsp)',
    'cups (cup)'
  ];

  categories = [
    'Vegetables',
    'Fruits',
    'Grains',
    'Proteins',
    'Dairy',
    'Spices',
    'Others'
  ];

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadIngredients();
  }

  private initForm(): void {
    this.ingredientForm = this.formBuilder.group({
      id: [null],
      name: ['', [Validators.required, Validators.minLength(2)]],
      unit: ['', Validators.required],
      quantity: [0, [Validators.required, Validators.min(0)]],
      calories: [0, [Validators.required, Validators.min(0)]],
      category: ['', Validators.required]
    });
  }

  private loadIngredients(): void {
    // TODO: Implement API call to load ingredients
    // For now, using mock data
    this.ingredients = [
      {
        id: 1,
        name: 'Tomato',
        unit: 'units (un)',
        quantity: 1,
        calories: 22,
        category: 'Vegetables'
      },
      {
        id: 2,
        name: 'Olive Oil',
        unit: 'milliliters (ml)',
        quantity: 15,
        calories: 120,
        category: 'Others'
      }
    ];
  }

  onSubmit(): void {
    if (this.ingredientForm.valid) {
      const ingredient = this.ingredientForm.value;
      if (this.isEditing) {
        this.updateIngredient(ingredient);
      } else {
        this.addIngredient(ingredient);
      }
    } else {
      this.markFormGroupTouched(this.ingredientForm);
      this.toastr.warning('Please fill in all required fields correctly', 'Validation Error');
    }
  }

  private addIngredient(ingredient: Ingredient): void {
    // TODO: Implement API call to add ingredient
    ingredient.id = this.ingredients.length + 1;
    this.ingredients.push(ingredient);
    this.toastr.success('Ingredient added successfully', 'Success');
    this.resetForm();
  }

  private updateIngredient(ingredient: Ingredient): void {
    // TODO: Implement API call to update ingredient
    const index = this.ingredients.findIndex(i => i.id === ingredient.id);
    if (index !== -1) {
      this.ingredients[index] = ingredient;
      this.toastr.success('Ingredient updated successfully', 'Success');
      this.resetForm();
    }
  }

  editIngredient(ingredient: Ingredient): void {
    this.isEditing = true;
    this.ingredientForm.patchValue(ingredient);
  }

  deleteIngredient(id: number): void {
    // TODO: Implement API call to delete ingredient
    this.ingredients = this.ingredients.filter(i => i.id !== id);
    this.toastr.success('Ingredient deleted successfully', 'Success');
  }

  resetForm(): void {
    this.isEditing = false;
    this.ingredientForm.reset();
    this.initForm();
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  getErrorMessage(fieldName: string): string {
    const control = this.ingredientForm.get(fieldName);
    if (!control) return '';

    if (control.hasError('required')) {
      return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} is required`;
    }
    if (control.hasError('minlength')) {
      return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} must be at least 2 characters`;
    }
    if (control.hasError('min')) {
      return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} must be greater than or equal to 0`;
    }
    return '';
  }

  sortIngredients(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.ingredients.sort((a: any, b: any) => {
      const valueA = a[column];
      const valueB = b[column];
      const direction = this.sortDirection === 'asc' ? 1 : -1;

      if (typeof valueA === 'string') {
        return valueA.localeCompare(valueB) * direction;
      }
      return (valueA - valueB) * direction;
    });
  }
}
