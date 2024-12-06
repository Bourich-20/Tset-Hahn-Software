using ExpenseTrackerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Repositories
{
    public interface IExpenseRepository
    {
        Task<Expense> AddExpenseAsync(Expense expense);
        Task<List<Expense>> GetExpensesAsync(string userId);
        Task DeleteExpenseAsync(int id, string userId);
        Task<Budget?> GetBudgetWithExpensesAsync(int budgetId);
    }
}
