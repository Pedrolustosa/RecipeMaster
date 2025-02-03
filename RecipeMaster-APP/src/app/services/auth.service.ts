import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest, RegisterRequest, TokenResponse, User, UpdateUserRequest } from '../models/auth.models';
import { environment } from '../../environments/environment';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = `${environment.apiUrl}/api/auth`;
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser$: Observable<User | null>;

  constructor(private http: HttpClient, private storageService: StorageService) {
    this.currentUserSubject = new BehaviorSubject<User | null>(this.getUserFromStorage());
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  login(credentials: LoginRequest): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${this.API_URL}/login`, credentials).pipe(
      tap(response => {
        if (response.token) {
          this.storageService.set('token', response.token);
          this.updateUserFromToken(response.token);
        }
      })
    );
  }

  register(userData: RegisterRequest): Observable<string> {
    return this.http.post(`${this.API_URL}/register`, userData, {
      responseType: 'text'
    });
  }

  logout(): void {
    this.storageService.remove('token');
    this.currentUserSubject.next(null);
  }

  isAuthenticated(): boolean {
    const token = this.storageService.get('token');
    return !!token && !this.isTokenExpired();
  }

  isTokenExpired(): boolean {
    const token = this.storageService.get('token');
    if (!token) return true;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const expiry = payload.exp * 1000;
      const timeUntilExpiry = expiry - Date.now();

      if (timeUntilExpiry > 0 && timeUntilExpiry <= 5 * 60 * 1000) {
        console.warn('Token will expire in less than 5 minutes');
      }

      return Date.now() >= expiry;
    } catch (error) {
      console.error('Error checking token expiration:', error);
      return true;
    }
  }

  getTokenExpirationTime(): number | null {
    const token = this.storageService.get('token');
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp * 1000;
    } catch {
      return null;
    }
  }

  private getUserFromStorage(): User | null {
    const token = this.storageService.get('token');
    if (token) {
      return this.parseUserFromToken(token);
    }
    return null;
  }

  private updateUserFromToken(token: string): void {
    const user = this.parseUserFromToken(token);
    if (user) {
      this.currentUserSubject.next(user);
    }
  }

  private parseUserFromToken(token: string): User | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        id: payload.nameid || payload.sub,
        email: payload.email,
        userName: payload.unique_name,
        isActive: payload.isActive
      };
    } catch (error) {
      console.error('Error parsing user from token:', error);
      return null;
    }
  }

  getUserById(id: string): Observable<User> {
    return this.http.get<User>(`${this.API_URL}/user/${id}`);
  }

  getUserByEmail(email: string): Observable<User> {
    return this.http.get<User>(`${this.API_URL}/user/email/${email}`);
  }

  getUserByUsername(username: string): Observable<User> {
    return this.http.get<User>(`${this.API_URL}/user/username/${username}`);
  }

  updateUser(id: string, userData: UpdateUserRequest): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/user/${id}`, userData).pipe(
      tap(() => {
        // Atualiza o usuário atual se for o mesmo que está sendo editado
        if (this.currentUserValue?.id === id) {
          const updatedUser = { ...this.currentUserValue, ...userData };
          this.currentUserSubject.next(updatedUser);
        }
      })
    );
  }

  deactivateUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/user/${id}`).pipe(
      tap(() => {
        // Se o usuário atual for desativado, faz logout
        if (this.currentUserValue?.id === id) {
          this.logout();
        }
      })
    );
  }
}
