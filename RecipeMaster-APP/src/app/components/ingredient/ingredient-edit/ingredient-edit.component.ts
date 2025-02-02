import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { IngredientService } from '../../../services/ingredient.service';
import { Ingredient } from '../../../models/ingredient.model';
import { MeasurementUnit } from '../../../models/measurement-unit.enum';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-ingredient-edit',
  templateUrl: './ingredient-edit.component.html',
  styleUrls: ['./ingredient-edit.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, NgxSpinnerModule, TranslateModule]
})
export class IngredientEditComponent implements OnInit {
  ingredientForm!: FormGroup;
  submitted = false;
  isLoading = false;
  originalIngredient: Ingredient | null = null;
  measurementUnits = Object.values(MeasurementUnit);

  // Field instructions
  fieldInstructions: any;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private ingredientService: IngredientService,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {
    this.initForm();
    this.initFieldInstructions();
  }

  private initFieldInstructions(): void {
    this.fieldInstructions = {
      name: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.NAME.HELP'),
      description: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.DESCRIPTION.HELP'),
      unit: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.UNIT.HELP'),
      cost: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.COST.HELP'),
      stockQuantity: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.STOCK_QUANTITY.HELP'),
      minimumStockLevel: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.MINIMUM_STOCK_LEVEL.HELP'),
      supplierName: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.SUPPLIER_NAME.HELP'),
      isPerishable: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.IS_PERISHABLE.HELP'),
      originCountry: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.ORIGIN_COUNTRY.HELP'),
      storageInstructions: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.STORAGE_INSTRUCTIONS.HELP'),
      isActive: this.translate.instant('INGREDIENTS.EDIT.FORM.FIELD_INSTRUCTIONS.IS_ACTIVE.HELP')
    };
  }

  private initForm(): void {
    this.ingredientForm = this.formBuilder.group({
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
      unit: ['', [Validators.required]],
      cost: ['', [
        Validators.required,
        Validators.min(0.01),
        Validators.max(9999.99)
      ]],
      stockQuantity: ['', [
        Validators.required,
        Validators.min(0),
        Validators.max(9999)
      ]],
      minimumStockLevel: ['', [
        Validators.required,
        Validators.min(0),
        Validators.max(9999)
      ]],
      supplierName: ['', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100)
      ]],
      isPerishable: [false],
      originCountry: ['', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100)
      ]],
      storageInstructions: [''],
      isActive: [true]
    });

    // Add conditional validation for storage instructions
    this.ingredientForm.get('isPerishable')?.valueChanges.subscribe(isPerishable => {
      const storageControl = this.ingredientForm.get('storageInstructions');
      if (isPerishable) {
        storageControl?.setValidators([
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(500)
        ]);
      } else {
        storageControl?.clearValidators();
      }
      storageControl?.updateValueAndValidity();
    });
  }

  ngOnInit(): void {
    this.loadIngredient();
  }

  // Getter for easy access to form fields
  get f() {
    return this.ingredientForm.controls;
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

  private loadIngredient(): void {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.isLoading = true;
      this.ingredientService.getById(id).subscribe({
        next: (ingredient: Ingredient) => {
          this.originalIngredient = ingredient;
          this.ingredientForm.patchValue({
            name: ingredient.name,
            description: ingredient.description,
            unit: ingredient.unit,
            cost: ingredient.cost,
            stockQuantity: ingredient.stockQuantity,
            minimumStockLevel: ingredient.minimumStockLevel,
            supplierName: ingredient.supplierName,
            isPerishable: ingredient.isPerishable,
            originCountry: ingredient.originCountry,
            storageInstructions: ingredient.storageInstructions,
            isActive: ingredient.isActive
          });
        },
        error: (error: Error) => {
          console.error('Error loading ingredient:', error);
          this.toastr.error('Failed to load ingredient', 'Error');
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    }
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.ingredientForm.invalid) {
      return;
    }

    this.isLoading = true;
    const id = this.route.snapshot.params['id'];

    this.ingredientService.update(id, this.ingredientForm.value).subscribe({
      next: () => {
        this.toastr.success('Ingredient updated successfully', 'Success');
        this.router.navigate(['/ingredients']);
      },
      error: (error: Error) => {
        this.isLoading = false;
        this.toastr.error('Failed to update ingredient', 'Error');
        console.error('Error updating ingredient:', error);
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/ingredients']);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}