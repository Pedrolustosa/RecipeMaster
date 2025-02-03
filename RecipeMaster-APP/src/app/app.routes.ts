import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { IngredientListComponent } from './components/ingredient/ingredient-list/ingredient-list.component';
import { IngredientCreateComponent } from './components/ingredient/ingredient-create/ingredient-create.component';
import { IngredientEditComponent } from './components/ingredient/ingredient-edit/ingredient-edit.component';
import { RegisterComponent } from './components/register/register.component';
import { authGuard } from './guards/auth.guard';
import { RecipeListComponent } from './components/recipe/recipe-list/recipe-list.component';
import { RecipeCreateComponent } from './components/recipe/recipe-create/recipe-create.component';
import { RecipeEditComponent } from './components/recipe/recipe-edit/recipe-edit.component';
import { ProfileComponent } from './components/profile/profile.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { 
    path: 'dashboard', 
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
  {
    path: 'recipes',
    canActivate: [authGuard],
    children: [
      { path: '', component: RecipeListComponent },
      { path: 'create', component: RecipeCreateComponent },
      { path: 'edit/:id', component: RecipeEditComponent }
    ]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [authGuard]
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];
