using ExpenseTrackerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Repositories
{
    public interface IExpenseRepository
    {
        Task<Expense> AddExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id, string userId);
         IQueryable<Expense> GetExpensesQueryable(string userId); 
        Task<Budget?> GetBudgetWithExpensesAsync(int budgetId);
    }
}
