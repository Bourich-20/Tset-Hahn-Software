using ExpenseTrackerAPI.DTO;
using ExpenseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Services
{
    public interface IExpenseService
    {
        Task<string> AddExpenseAsync(ExpenseDTO expenseDTO, string userId);

        Task<List<ExpenseDTO>> GetExpensesAsync(string userId);

        Task DeleteExpenseAsync(int id, string userId);
    }
}
