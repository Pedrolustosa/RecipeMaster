import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { RegisterRequest } from '../../models/auth.models';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    ToastrModule,
    RouterModule
  ]
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = '';
  loading: boolean = false;
  showPassword: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private authService: AuthService
  ) {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;
    this.spinner.show();
    this.errorMessage = '';

    const userData: RegisterRequest = {
      email: this.registerForm.get('email')?.value,
      password: this.registerForm.get('password')?.value
    };

    this.authService.register(userData).subscribe({
      next: () => {
        this.spinner.hide();
        this.loading = false;
        this.toastr.success('Account created successfully!', 'Success');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.spinner.hide();
        this.loading = false;
        
        if (error.status === 422) { // UnprocessableEntity
          const errorDetails = error.error?.details;
          if (errorDetails) {
            const errorMessages = Object.values(errorDetails).flat();
            this.errorMessage = errorMessages.join(', ');
          } else {
            this.errorMessage = error.error?.error || 'Invalid data provided. Please check your information.';
          }
        } else if (error.status === 500) { // InternalServerException
          this.errorMessage = 'Internal server error. Please try again later.';
        } else {
          this.errorMessage = error.error || 'Failed to create account. Please try again.';
        }
        
        this.toastr.error(this.errorMessage, 'Error');
      }
    });
  }
}
