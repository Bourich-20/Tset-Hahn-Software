import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5062/api/User';
  private token: string | null = null; 

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { email, password });
  }

  setToken(token: string) {
    this.token = token;
  }

  getToken(): string | null {
    return this.token;
  }

  isAuthenticated(): boolean {
    return !!this.token;
  }
  register(user: { email: string; firstName: string; lastName: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }
}
