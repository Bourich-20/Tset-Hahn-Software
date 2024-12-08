import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ExpenseService } from '../../services/expense.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-view-expenses',
  templateUrl: './view-expenses.component.html',
  styleUrls: ['./view-expenses.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CurrencyPipe,
    DatePipe
  ]
})
export class ViewExpensesComponent implements OnInit {
  expenseForm!: FormGroup;
  categories: string[] = ["All", 'Food', 'Transport', 'Entertainment'];
  selectedCategory: string | null = "";
  expenses: any[] = [];
  currentPage: number = 1;
  totalPages: number = 1;
  pages: number[] = [];

  constructor(
    private expenseService: ExpenseService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.expenseForm = this.fb.group({
      category: ['All'],
      startDate: ['']
    });
    this.fetchExpenses();
  }

  fetchExpenses(): void {
    const { category, startDate } = this.expenseForm.value;
    const params = {
      PageNumber: this.currentPage,
      PageSize: 5,
      Category: category === 'All' ? '' : category,
      Date: startDate || ''     
    };

    this.expenseService.getExpenses(params).subscribe(
      (data) => {
        this.expenses = data?.data?.expenses || [];  
        this.totalPages = data?.data?.totalPages || 1; 
        console.log("Expenses Data:", data);

        this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
      },
      (error) => {
        console.error('Error retrieving expenses:', error);
      }
    );
  }

  onSubmit(): void {
    if (this.expenseForm.invalid) {
      console.warn('Category or Date is missing');
      return;
    }
    this.fetchExpenses();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.fetchExpenses();
  }

  deleteExpense(id: number): void {
    if (confirm('Are you sure you want to delete this expense?')) {
      this.expenseService.deleteExpense(id).subscribe(
        () => {
          this.expenses = this.expenses.filter(exp => exp.id !== id);
          console.log('Expense deleted successfully');
        },
        (error) => {
          console.error('Error deleting expense:', error);
        }
      );
    }
  }
}
