import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Recipe, CreateRecipeRequest, UpdateRecipeRequest } from '../models/recipe.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private readonly API_URL = `${environment.apiUrl}/api/recipe`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(this.API_URL);
  }

  getById(id: string): Observable<Recipe> {
    return this.http.get<Recipe>(`${this.API_URL}/${id}`);
  }

  create(recipe: CreateRecipeRequest): Observable<string> {
    return this.http.post<string>(this.API_URL, recipe);
  }

  update(id: string, recipe: UpdateRecipeRequest): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, recipe);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
