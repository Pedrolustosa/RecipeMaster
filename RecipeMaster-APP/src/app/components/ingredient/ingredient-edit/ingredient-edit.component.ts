import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { IngredientService } from '../../../services/ingredient.service';
import { Ingredient } from '../../../models/ingredient.model';

@Component({
  selector: 'app-ingredient-edit',
  templateUrl: './ingredient-edit.component.html',
  styleUrls: ['./ingredient-edit.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, NgxSpinnerModule]
})
export class IngredientEditComponent implements OnInit {
  ingredientForm: FormGroup;
  submitted = false;
  isLoading = false;
  originalIngredient: Ingredient | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private ingredientService: IngredientService,
    private toastr: ToastrService
  ) {
    this.ingredientForm = this.formBuilder.group({
      id: [''],
      name: ['', [Validators.required, Validators.minLength(2)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      unit: ['', Validators.required],
      cost: [0, [Validators.required, Validators.min(0)]],
      stockQuantity: [0, [Validators.required, Validators.min(0)]],
      minimumStockLevel: [0, [Validators.required, Validators.min(0)]],
      supplierName: ['', Validators.required],
      isPerishable: [false],
      originCountry: ['', Validators.required],
      storageInstructions: [''],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.loadIngredient();
  }

  // Getter for easy access to form fields
  get f() {
    return this.ingredientForm.controls;
  }

  private loadIngredient(): void {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.isLoading = true;
      this.ingredientService.getById(id).subscribe({
        next: (ingredient: Ingredient) => {
          this.originalIngredient = ingredient;
          this.ingredientForm.patchValue({
            id: ingredient.id,
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
        console.error('Error updating ingredient:', error);
        this.toastr.error('Failed to update ingredient', 'Error');
      },
      complete: () => {
        this.isLoading = false;
      }
    });
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
