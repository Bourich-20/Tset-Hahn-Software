import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { BudgetService } from '../services/budget.service';
import { NotificationService } from '../../../shared/services/notification.service';
import { ReactiveFormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-budget',
  templateUrl: './budget.component.html',
  styleUrls: ['./budget.component.scss'],
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
})
export class BudgetComponent implements OnInit {
  budgetForm: FormGroup; 
  budgets: any[] = [];
  filteredBudgets: any[] = [];
  isAddModalOpen = false;
  isEditModalOpen = false;
  isDeleteModalOpen = false;
  selectedBudgetId: string | null = null;
  minDate: string; 
  minEndDate: string;
  searchMonth: string = ''; 

  constructor(
    private fb: FormBuilder, 
    private budgetService: BudgetService,
    private notificationService: NotificationService
  ) {
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0]; 
    this.minEndDate = this.minDate; 
    this.budgetForm = this.fb.group({
      amount: ['', [Validators.required, Validators.min(1)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.getBudgets();
  }

  getBudgets(): void {
    this.budgetService.getBudgets().subscribe(
      (data) => {
        this.budgets = data;
        this.filteredBudgets = [...this.budgets];
      },
      (error) => {
        this.notificationService.showError('Erreur de récupération des budgets');
      }
    );
  }

  filterByDate(): void {
    if (this.searchMonth) {
      const [year, month] = this.searchMonth.split('-').map(Number);
      this.filteredBudgets = this.budgets.filter(budget => {
        const startDate = new Date(budget.startDate);
        const endDate = new Date(budget.endDate);
        return (
          (startDate.getFullYear() === year && startDate.getMonth() + 1 === month) ||
          (endDate.getFullYear() === year && endDate.getMonth() + 1 === month)
        );
      });
    } else {
      this.filteredBudgets = [...this.budgets];
    }
  }

  openAddModal() {
    this.isAddModalOpen = true;
  }

  closeAddModal() {
    this.isAddModalOpen = false;
  }

  closeEditModal() {
    this.isEditModalOpen = false;
  }

  openDeleteModal(budgetId: string) {
    this.isDeleteModalOpen = true;
    this.selectedBudgetId = budgetId;
  }

  closeDeleteModal() {
    this.isDeleteModalOpen = false;
  }

  onSubmit() {
    if (this.budgetForm.valid) {
      this.budgetService.addBudget(this.budgetForm.value).subscribe(
        () => {
          this.notificationService.showSuccess('Budget ajouté avec succès');
          this.closeAddModal();
          this.getBudgets();
        },
        () => {
          this.notificationService.showError('Échec de l\'ajout du budget');
        }
      );
    }
  }

  onSubmitEdit() {
    if (this.budgetForm.valid && this.selectedBudgetId) {
      this.budgetService.updateBudget(this.selectedBudgetId, this.budgetForm.value).subscribe(
        () => {
          this.notificationService.showSuccess('Budget modifié avec succès');
          this.closeEditModal();
          this.getBudgets();
        },
        () => {
          this.notificationService.showError('Échec de la modification du budget');
        }
      );
    }
  }

  openEditModal(budgetId: string) {
    this.isEditModalOpen = true;
    this.selectedBudgetId = budgetId;

    const selectedBudget = this.budgets.find(budget => budget.id === budgetId);

    if (selectedBudget) {
      const startDate = this.formatDate(selectedBudget.startDate);
      const endDate = this.formatDate(selectedBudget.endDate);

      this.budgetForm.patchValue({
        amount: selectedBudget.amount,
        startDate: startDate,
        endDate: endDate
      });

      this.minEndDate = startDate;
    }
  }

  formatDate(date: string): string {
    const parsedDate = new Date(date);
    return parsedDate.toISOString().split('T')[0];
  }

  onStartDateChange(event: any) {
    this.minEndDate = event.target.value;
  }

  onDelete() {
    if (this.selectedBudgetId) {
      this.budgetService.deleteBudget(this.selectedBudgetId).subscribe(
        () => {
          this.notificationService.showSuccess('Budget supprimé avec succès');
          this.closeDeleteModal();
          this.getBudgets();
        },
        () => {
          this.notificationService.showError('Échec de la suppression du budget');
        }
      );
    }
  }
}
