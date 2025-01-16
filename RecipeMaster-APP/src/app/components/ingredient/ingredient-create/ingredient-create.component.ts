import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { IngredientService } from '../../../services/ingredient.service';
import { CreateIngredientCommand } from '../../../models/ingredient.model';
import { NgxSpinnerModule } from 'ngx-spinner';

@Component({
  selector: 'app-ingredient-create',
  templateUrl: './ingredient-create.component.html',
  styleUrls: ['./ingredient-create.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, NgxSpinnerModule]
})
export class IngredientCreateComponent implements OnInit {
  ingredientForm: FormGroup;
  submitted = false;
  isLoading = false;

  constructor(
    private formBuilder: FormBuilder,
    private ingredientService: IngredientService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.ingredientForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      unit: ['', Validators.required],
      cost: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {}

  // Getter for easy access to form fields
  get f() {
    return this.ingredientForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.ingredientForm.invalid) {
      this.markFormGroupTouched(this.ingredientForm);
      return;
    }

    this.isLoading = true;
    const command: CreateIngredientCommand = this.ingredientForm.value;
    
    this.ingredientService.create(command).subscribe({
      next: () => {
        this.toastr.success('Ingredient created successfully', 'Success');
        this.router.navigate(['/ingredients']);
      },
      error: (error: Error) => {
        this.isLoading = false;
        this.toastr.error('Failed to create ingredient', 'Error');
        console.error('Error creating ingredient:', error);
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
