import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../app/features/auth/login/login.component';
import { RegisterComponent } from '../app/features/auth/register/register.component';
import { HomeComponent } from './features/home/home.component';
import { AddExpenseComponent } from '../app/features/components/add-expense/add-expense.component';
import { ViewExpensesComponent } from '../app/features/components/view-expenses/view-expenses.component';
import { BudgetProgressComponent } from '../app/features/components/budget-progress/budget-progress.component';
import { SettingsComponent } from '../app/features/components/settings/settings.component';
import { BudgetComponent } from './features/components/budget/budget.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'home',
    component: HomeComponent,
    children: [
      { path: 'add-expense', component: AddExpenseComponent },
      { path: 'view-expenses', component: ViewExpensesComponent },
      { path: 'budget-progress', component: BudgetProgressComponent },
      { path: 'settings', component: SettingsComponent },
      { path: 'budgets', component: BudgetComponent },
    ],
  },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
