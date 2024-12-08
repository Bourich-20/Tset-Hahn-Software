using ExpenseTrackerAPI.DTO;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories;
using System;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetService(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<ResponseBudgetRequest> AddBudgetAsync(AddBudgetRequest budgetRequest,  string  userId)
        {
            return await _budgetRepository.AddBudgetAsync(budgetRequest,userId);
        }

        public async Task<ResponseBudgetRequest> UpdateBudgetAsync(int budgetId, EditBudgetRequest budgetRequest)
        {
            return await _budgetRepository.UpdateBudgetAsync(budgetId,budgetRequest);
        }

     public async Task<Budget> GetBudgetByIdAsync(int budgetId)
{
    var budget = await _budgetRepository.GetBudgetByIdAsync(budgetId);
    if (budget == null)
    {
        throw new KeyNotFoundException("Budget not found.");
    }
    return budget;
}

public async Task<Budget> GetActiveBudgetForUserAsync(string userId)
{
    var budget = await _budgetRepository.GetActiveBudgetForUserAsync(userId);
    if (budget == null)
    {
        throw new KeyNotFoundException("Active budget not found for the user.");
    }
    return budget;
}

        public async Task<string> DeleteBudgetAsync(int budgetId)
        {
            return await _budgetRepository.DeleteBudgetAsync(budgetId);
        }

         public async Task<List<BudgetWithTotalExpensesDTO>> GetBudgetsWithTotalExpensesAsync(string userId)
        {
            var budgets = await _budgetRepository.GetBudgetsByUserIdAsync(userId);
            if (budgets == null || !budgets.Any())
            {
                throw new KeyNotFoundException("No budgets found for the user.");
            }

            var budgetWithTotalExpenses = budgets.Select(budget => new BudgetWithTotalExpensesDTO
            {
                Id = budget.Id,
                Amount = budget.Amount,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate,
                TotalExpenses = budget.Expenses
                    .Where(e => e.Date >= budget.StartDate && e.Date <= budget.EndDate)
                    .Sum(e => e.Amount),
            }).ToList();

            return budgetWithTotalExpenses;
        }
    }
}
