import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { IngredientListComponent } from './components/ingredient/ingredient-list/ingredient-list.component';
import { IngredientCreateComponent } from './components/ingredient/ingredient-create/ingredient-create.component';
import { IngredientEditComponent } from './components/ingredient/ingredient-edit/ingredient-edit.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: '', 
    component: DashboardComponent,
    canActivate: [authGuard]
  },
  {
    path: 'ingredients',
    canActivate: [authGuard],
    children: [
      { path: '', component: IngredientListComponent },
      { path: 'create', component: IngredientCreateComponent },
      { path: 'edit/:id', component: IngredientEditComponent }
    ]
  },
  { path: '**', redirectTo: '' }
];
