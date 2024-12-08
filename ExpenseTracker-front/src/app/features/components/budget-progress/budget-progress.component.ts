import { Component, OnInit } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions, ChartData } from 'chart.js';
import { ExpenseService } from '../../services/expense.service';
import { FormsModule } from '@angular/forms';
import { Chart, PieController, ArcElement, Tooltip, Legend } from 'chart.js';
import { CommonModule } from '@angular/common';

interface Expense {
  category: string;
  totalAmount: number;
  date: string;
}

Chart.register(PieController, ArcElement, Tooltip, Legend);

@Component({
  selector: 'app-budget-progress',
  templateUrl: './budget-progress.component.html',
  styleUrls: ['./budget-progress.component.scss'],
  standalone: true,
  imports: [BaseChartDirective, FormsModule, CommonModule],
})
export class BudgetProgressComponent implements OnInit {
  public chartType: 'pie' = 'pie';
  public chartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      },
      tooltip: {
        callbacks: {
          label: (tooltipItem) => {
            const category = tooltipItem.label || '';
            const value = tooltipItem.raw as number;
            if (this.selectedCategory === 'All') {
              return `${category}: ${value.toFixed(2)} USD`;
            }
            const index = tooltipItem.dataIndex;
            const date = this.dates[index];
            const formattedDate = this.formatDateTime(date); 
            return `${category}: ${value.toFixed(2)} USD\n${formattedDate}`;
          },
        },
      },
    },
  };

  public chartData: ChartData<'pie'> = { labels: [], datasets: [] };

  public startDate!: string;
  public endDate!: string;
  public selectedCategory: string = 'All';
  public categories: string[] = ['All', 'Food', 'Transport', 'Entertainment'];
  public categoryDropdownOpen: boolean = false;

  public monthlyBudget: number = 1000;
  public totalExpenses: number = 0;
  public budgetExceeded: boolean = false;

  private dates: string[] = []; 

  constructor(private expenseService: ExpenseService) {}

  ngOnInit(): void {
    const today = new Date();
    this.endDate = today.toISOString().split('T')[0];
    today.setMonth(today.getMonth() - 1);
    this.startDate = today.toISOString().split('T')[0];
    this.loadChartData();
  }

  loadChartData(): void {
    const params = {
      Category: this.selectedCategory === 'All' ? '' : this.selectedCategory,
      StartDate: this.startDate,
      EndDate: this.endDate,
    };

    this.expenseService.getExpensesCategoryAmounts(params).subscribe({
      next: (response) => {
        const expenses = response.data as Expense[];

        let groupedExpenses: { [key: string]: { [key: string]: number[] } } = {};
        expenses.forEach((exp) => {
          if (this.selectedCategory === 'All' || exp.category === this.selectedCategory) {
            if (!groupedExpenses[exp.category]) {
              groupedExpenses[exp.category] = {};
            }

            if (!groupedExpenses[exp.category][exp.date]) {
              groupedExpenses[exp.category][exp.date] = [];
            }

            groupedExpenses[exp.category][exp.date].push(exp.totalAmount);
          }
        });

        let allCategories: string[] = [];
        let allAmounts: number[] = [];
        let allDates: string[] = [];

        if (this.selectedCategory === 'All') {
          Object.keys(groupedExpenses).forEach((category) => {
            const amounts = groupedExpenses[category];
            const sum = Object.values(amounts).flat().reduce((acc, val) => acc + val, 0);
            allCategories.push(category);
            allAmounts.push(sum);
          });
        } else {
          Object.keys(groupedExpenses).forEach((category) => {
            Object.keys(groupedExpenses[category]).forEach((date) => {
              const amounts = groupedExpenses[category][date];
              const sum = amounts.reduce((acc, val) => acc + val, 0);
              allCategories.push(category);
              allAmounts.push(sum);
              allDates.push(date);
            });
          });
        }

        this.totalExpenses = allAmounts.reduce((acc, val) => acc + val, 0);
        this.budgetExceeded = this.totalExpenses > this.monthlyBudget;

        this.chartData = {
          labels: allCategories,
          datasets: [
            {
              data: allAmounts,
              backgroundColor: this.generateColors(allCategories.length),
              hoverBackgroundColor: this.generateColors(allCategories.length),
            },
          ],
        };

        this.dates = allDates; 
      },
      error: (err) => {
        console.error('Erreur lors de la récupération des données :', err);
      },
    });
  }

  onDateChange(): void {
    this.loadChartData();
  }

  toggleCategoryDropdown(): void {
    this.categoryDropdownOpen = !this.categoryDropdownOpen;
  }

  selectCategory(category: string): void {
    this.selectedCategory = category;
    this.categoryDropdownOpen = false;
    this.loadChartData();
  }

  private generateColors(count: number): string[] {
    const colorsForAll = ['#FF5733', '#33FF57', '#3357FF'];
    if (this.selectedCategory === 'All') {
      return colorsForAll.slice(0, count);
    }
    const colors = ['#FF5733', '#33FF57', '#3357FF', '#FF33A5', '#33FFF0'];
    return Array.from({ length: count }, (_, i) => colors[i % colors.length]);
  }

  onCategoryChange(): void {
    this.loadChartData();
  }

  private formatDateTime(dateTime: string): string {
    const date = new Date(dateTime);
    const formattedDate = date.toLocaleDateString();
    const formattedTime = date.toLocaleTimeString(); 
    return `Date: ${formattedDate} Time: ${formattedTime}`;
  }
}
