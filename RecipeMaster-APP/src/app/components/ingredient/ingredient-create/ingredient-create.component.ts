import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { IngredientService } from '../../../services/ingredient.service';
import { MeasurementUnit } from '../../../models/measurement-unit.enum';
import { NgxSpinnerModule } from 'ngx-spinner';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-ingredient-create',
  templateUrl: './ingredient-create.component.html',
  styleUrls: ['./ingredient-create.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, NgxSpinnerModule, TranslateModule]
})
export class IngredientCreateComponent implements OnInit {
  ingredientForm!: FormGroup;
  submitted = false;
  loading = false;
  measurementUnits = Object.values(MeasurementUnit);
  fieldInstructions: any;

  constructor(
    private formBuilder: FormBuilder,
    private ingredientService: IngredientService,
    private router: Router,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {
    this.initForm();
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

  ngOnInit(): void { }

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

    return 'Invalid field';
  }

  async onSubmit(): Promise<void> {
    this.submitted = true;

    if (this.ingredientForm.invalid) {
      this.toastr.error('Please correct the errors in the form before submitting.');
      return;
    }

    try {
      this.loading = true;
      const formValue = this.ingredientForm.value;

      await firstValueFrom(this.ingredientService.create(formValue));
      this.toastr.success('Ingredient created successfully!');
      this.router.navigate(['/ingredients']);
    } catch (error) {
      this.toastr.error('Failed to create ingredient. Please try again.');
    } finally {
      this.loading = false;
    }
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
