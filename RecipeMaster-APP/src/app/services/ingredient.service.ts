import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Ingredient, CreateIngredientCommand, UpdateIngredientDTO } from '../models/ingredient.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  private apiUrl = `${environment.apiUrl}/api/ingredient`;
  private ingredientsSubject = new BehaviorSubject<Ingredient[]>([]);
  public ingredients$ = this.ingredientsSubject.asObservable();

  constructor(private http: HttpClient) {}

  getAll(): Observable<Ingredient[]> {
    return this.http.get<Ingredient[]>(this.apiUrl).pipe(
      tap(ingredients => this.ingredientsSubject.next(ingredients)),
      catchError(this.handleError)
    );
  }

  getById(id: string): Observable<Ingredient> {
    return this.http.get<Ingredient>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  create(command: CreateIngredientCommand): Observable<string> {
    return this.http.post<string>(this.apiUrl, command).pipe(
      tap(() => this.refreshIngredients()),
      catchError(this.handleError)
    );
  }

  update(id: string, dto: UpdateIngredientDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto).pipe(
      tap(() => this.refreshIngredients()),
      catchError(this.handleError)
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => {
        const currentIngredients = this.ingredientsSubject.value;
        this.ingredientsSubject.next(
          currentIngredients.filter(ingredient => ingredient.id !== id)
        );
      }),
      catchError(this.handleError)
    );
  }

  private refreshIngredients(): void {
    this.getAll().subscribe();
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      if (error.status === 404) {
        errorMessage = 'Resource not found';
      } else if (error.status === 422) {
        errorMessage = error.error?.error || 'Validation failed';
      } else if (error.status === 500) {
        errorMessage = 'Internal server error';
      }
    }

    return throwError(() => new Error(errorMessage));
  }
}
