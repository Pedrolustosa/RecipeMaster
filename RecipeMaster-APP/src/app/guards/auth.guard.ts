import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    if (authService.isTokenExpired()) {
      authService.logout();
      router.navigate(['/login']);
      return false;
    }
    return true;
  }

  router.navigate(['/login']);
  return false;
};
