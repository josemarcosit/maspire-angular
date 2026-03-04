import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../shared/config/environment';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  fullName: string;
  password: string;
}

export interface AuthResponse {
  success: boolean;
  message: string;
  token: string;
  user: UserInfo;
}

export interface UserInfo {
  id: number;
  email: string;
  fullName: string;
  role: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/api/auth`;
  private currentUserSubject = new BehaviorSubject<UserInfo | null>(null);
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);

  public currentUser$ = this.currentUserSubject.asObservable();
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        if (response.success && response.token) {
          this.setToken(response.token);
          this.setCurrentUser(response.user);
        }
      }),
    );
  }

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, data).pipe(
      tap((response) => {
        if (response.success && response.token) {
          this.setToken(response.token);
          this.setCurrentUser(response.user);
        }
      }),
    );
  }

  logout(): void {
    const token = localStorage.getItem('token');
    if (token) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      this.currentUserSubject.next(null);
      this.isLoggedInSubject.next(false);
    }
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getCurrentUser(): UserInfo | null {
    return this.currentUserSubject.getValue();
  }

  private setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  private setCurrentUser(user: UserInfo): void {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSubject.next(user);
    this.isLoggedInSubject.next(true);
  }

  private loadUserFromStorage(): void {
    const token = this.getToken();
    const userStr = localStorage.getItem('user');

    if (token && userStr) {
      try {
        const user = JSON.parse(userStr);
        this.currentUserSubject.next(user);
        this.isLoggedInSubject.next(true);
      } catch (e) {
        this.logout();
      }
    }
  }
}
