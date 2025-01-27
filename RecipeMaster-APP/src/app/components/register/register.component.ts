import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { RegisterRequest } from '../../models/auth.models';
import { TranslateService, TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    ToastrModule,
    RouterModule,
    TranslateModule
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
    private authService: AuthService,
    private translate: TranslateService
  ) {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
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
        this.toastr.success(
          this.translate.instant('AUTH.REGISTER.MESSAGES.SUCCESS'),
          this.translate.instant('COMMON.SUCCESS')
        );
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.spinner.hide();
        this.loading = false;
        this.errorMessage = error?.error?.message || this.translate.instant('AUTH.REGISTER.MESSAGES.ERROR');
        this.toastr.error(
          this.errorMessage,
          this.translate.instant('COMMON.ERROR')
        );
      }
    });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  getValidationMessage(field: string): string {
    const control = this.registerForm.get(field);
    if (!control?.errors) return '';

    if (control.errors['required']) {
      return this.translate.instant(`AUTH.REGISTER.VALIDATION.${field.toUpperCase()}_REQUIRED`);
    }
    if (control.errors['email']) {
      return this.translate.instant('AUTH.REGISTER.VALIDATION.EMAIL_INVALID');
    }
    if (control.errors['minlength']) {
      return this.translate.instant('AUTH.REGISTER.VALIDATION.PASSWORD_MIN_LENGTH');
    }

    return '';
  }
}
