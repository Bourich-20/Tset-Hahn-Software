import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AuthModule } from '../app/features/auth/auth.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from './shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component'; 
import { SidebarComponent } from './shared/sidebar/sidebar.component';
import { AddExpenseComponent } from '../app/features/components/add-expense/add-expense.component';  
import { ViewExpensesComponent } from '../app/features/components/view-expenses/view-expenses.component';  
import { BudgetProgressComponent } from '../app/features/components/budget-progress/budget-progress.component';  
import { SettingsComponent } from '../app/features/components/settings/settings.component'; 
import { CommonModule } from '@angular/common';
import { AuthInterceptor } from '../app/features/auth/auth.interceptor';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    HttpClientModule,
    SharedModule,
    ReactiveFormsModule,
    AppComponent,  
    SidebarComponent,  
    AddExpenseComponent,  
    ViewExpensesComponent, 
    BudgetProgressComponent, 
    SettingsComponent  
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
