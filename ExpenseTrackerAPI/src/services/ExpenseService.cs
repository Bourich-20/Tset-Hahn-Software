using ExpenseTrackerAPI.DTO;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IBudgetRepository _budgetRepository;
       public ExpenseService(IExpenseRepository expenseRepository, IBudgetRepository budgetRepository)
{
    _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
    _budgetRepository = budgetRepository ?? throw new ArgumentNullException(nameof(budgetRepository));
}

         public async Task<string> AddExpenseAsync(ExpenseDTO expenseDTO, string userId)
        {
            // Récupérer le budget actif pour l'utilisateur
            var budget = await _budgetRepository.GetActiveBudgetForUserAsync(userId);

            if (budget == null)
                throw new InvalidOperationException("Budget not found for the user.");

            // Calculer les dépenses totales actuelles
            decimal totalExpenses = budget.Expenses.Sum(e => e.Amount);

            // Vérifier si l'ajout de la dépense dépasse le budget
            if (totalExpenses + expenseDTO.Amount > budget.Amount)
            {
                // Envoyer une alerte
                throw new InvalidOperationException("Total expenses exceed the monthly budget.");
            }

            // Créer la nouvelle dépense
            var expense = new Expense
            {
                Amount = expenseDTO.Amount,
                Category = expenseDTO.Category,
                Description = expenseDTO.Description,
                UserId = userId,
                BudgetId = budget.Id,
                Date = DateTime.UtcNow
            };

            // Ajouter la dépense dans la base de données
            await _expenseRepository.AddExpenseAsync(expense);

            return "Expense added successfully.";
        }
        public async Task<List<ExpenseDTO>> GetExpensesAsync(string userId)
        {
            // Retrieve expenses for the specified user from the repository
            var expenses = await _expenseRepository.GetExpensesAsync(userId);

            // Map the expenses to DTOs
            return expenses.Select(e => new ExpenseDTO
            {
                Amount = e.Amount,
                Category = e.Category,
                BudgetId = e.BudgetId
            }).ToList();
        }

        public async Task DeleteExpenseAsync(int id, string userId)
        {
            // Use the repository to delete the expense
            await _expenseRepository.DeleteExpenseAsync(id, userId);
        }
    }
}
