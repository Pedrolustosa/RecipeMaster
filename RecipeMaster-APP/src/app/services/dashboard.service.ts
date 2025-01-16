import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { 
  IngredientUsageDTO, 
  IngredientCostDTO 
} from '../models/dashboard.models';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = `${environment.apiUrl}/api/dashboard`;

  constructor(private http: HttpClient) { }

  getTotalRecipes(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-recipes`);
  }

  getTotalIngredients(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-ingredients`);
  }

  getAverageRecipeCost(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/average-recipe-cost`);
  }

  getTotalRecipeCost(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-recipe-cost`);
  }

  getMostExpensiveIngredients(): Observable<IngredientCostDTO[]> {
    return this.http.get<IngredientCostDTO[]>(`${this.apiUrl}/most-expensive-ingredients`);
  }

  getMostUsedIngredients(): Observable<IngredientUsageDTO[]> {
    return this.http.get<IngredientUsageDTO[]>(`${this.apiUrl}/most-used-ingredients`);
  }
}
