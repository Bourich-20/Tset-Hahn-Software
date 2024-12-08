import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BudgetService {

  private apiUrl = 'http://localhost:5062/api/Budget'; 
  constructor(private http: HttpClient) {}

  addBudget(budget: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, budget);
  }

  getBudgets(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/with-total-expenses`);
  }


  updateBudget(budgetId: string, budget: any): Observable<any> {
    console.log("budgetId"+budgetId);
    console.log("budget",budget);

    return this.http.put<any>(`${this.apiUrl}/${budgetId}`, budget);
  }
  
  deleteBudget(budgetId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${budgetId}`);
  }
}
