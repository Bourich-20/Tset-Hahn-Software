import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders,HttpParams  } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  private apiUrl = 'http://localhost:5062/api/Expense'; 
  
  constructor(private http: HttpClient,private authService: AuthService) {}

  addExpense(expense: any): Observable<any> {
    const token = this.authService.getToken();
    
    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${token}`)
      .set('Content-Type', 'application/json');
  
    return this.http.post(this.apiUrl, expense, { headers }).pipe(
      catchError((error) => {
        if (error.status === 401) {
          console.error('Unauthorized, please login again.');
        }
        return throwError(error);
      })
    );
  }
  getExpenses(params: any): Observable<any> {
    const token = this.authService.getToken();

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.get<any>(this.apiUrl, { params, headers }).pipe(
      catchError((error) => {
        console.error('Error fetching expenses', error);
        return throwError(error);
      })
    );
  }
  deleteExpense(id: number): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.delete(`${this.apiUrl}/${id}`, { headers }).pipe(
      catchError((error) => {
        console.error('Error deleting expense', error);
        return throwError(error);
      })
    );
  }
  getExpensesCategoryAmounts(params: any): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.get<any>(`${this.apiUrl}/category-amounts`, { params, headers }).pipe(
      catchError((error) => {
        console.error('Error fetching expenses', error);
        return throwError(error);
      })
    );
  }
}
