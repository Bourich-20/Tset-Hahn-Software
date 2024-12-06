using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTrackerAPI.Data;

namespace ExpenseTrackerAPI.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public ExpenseRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<List<Expense>> GetExpensesAsync(string userId)
        {
            return await _context.Expenses
                                 .Where(e => e.UserId == userId)
                                 .ToListAsync();
        }

        public async Task DeleteExpenseAsync(int id, string userId)
        {
            var expense = await _context.Expenses
                                         .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (expense == null)
            {
                throw new InvalidOperationException("Expense not found.");
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }

        // Implementation of method to get a budget with its associated expenses
    public async Task<Budget?> GetBudgetWithExpensesAsync(int budgetId)
{
    return await _context.Budgets
                         .Include(b => b.Expenses)
                         .FirstOrDefaultAsync(b => b.Id == budgetId);
}

    }
}
