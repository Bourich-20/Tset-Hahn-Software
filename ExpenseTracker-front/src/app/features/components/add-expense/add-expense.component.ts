import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ExpenseService } from '../../services/expense.service';
import { ReactiveFormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-expense',
  templateUrl: './add-expense.component.html',
  styleUrls: ['./add-expense.component.scss'],
  imports: [ ReactiveFormsModule, CommonModule ],
  standalone: true,
})
export class AddExpenseComponent {
  expenseForm: FormGroup;
  categories = ['Food', 'Transport', 'Entertainment'];
  
  constructor(
    private fb: FormBuilder,
    private expenseService: ExpenseService,
    private router: Router
  ) {
    this.expenseForm = this.fb.group({
      amount: [0, [Validators.required, Validators.min(0)]],
      category: ['Food', [Validators.required]],
      description: ['', [Validators.required]],
      budgetId: [null, [Validators.required]],
    });
  }

  onSubmit() {
    if (this.expenseForm.invalid) {
      return;
    }
    
    const expenseData = this.expenseForm.value;
    this.expenseService.addExpense(expenseData).subscribe({
      next: (response) => {
        console.log('Expense added:', response);
        this.router.navigate(['/home/view-expenses']);
      },
      error: (err) => {
        console.error('Error adding expense:', err);
      },
    });
  }
}
