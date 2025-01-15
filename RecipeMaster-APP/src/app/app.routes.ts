import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { 
    path: 'dashboard',
    redirectTo: '/login', // Temporary redirect until dashboard is implemented
    pathMatch: 'full'
  },
  { path: '**', redirectTo: '/login' }
];
