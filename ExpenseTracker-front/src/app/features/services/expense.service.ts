import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  private apiUrl = 'http://localhost:5062/api/expense'; 
  
  constructor(private http: HttpClient) {}

  addExpense(expense: any): Observable<any> {
    const token = localStorage.getItem('token');
    
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
  
}
