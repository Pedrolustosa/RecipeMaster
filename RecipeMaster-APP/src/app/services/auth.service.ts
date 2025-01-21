import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest, RegisterRequest, TokenResponse, User } from '../models/auth.models';
import { environment } from '../../environments/environment';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = `${environment.apiUrl}/api/auth`;
  private readonly TOKEN_EXPIRATION_TIME = 120; // 2 hours in minutes
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
      const expiry = payload.exp * 1000; // Convert to milliseconds
      const timeUntilExpiry = expiry - Date.now();
      
      // Avisa 5 minutos antes da expiração
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
      return payload.exp * 1000; // Convert to milliseconds
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
        email: payload.email,
        userName: payload[`http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name`]
      };
    } catch (error) {
      return null;
    }
  }
}
