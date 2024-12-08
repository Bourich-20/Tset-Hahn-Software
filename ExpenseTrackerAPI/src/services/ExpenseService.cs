using Microsoft.EntityFrameworkCore;  // Make sure to add this using directive
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
            var budget = await _budgetRepository.GetActiveBudgetForUserAsync(userId);

            if (budget == null)
                throw new InvalidOperationException("Budget not found for the user.");

            decimal totalExpenses = budget.Expenses.Sum(e => e.Amount);

            if (totalExpenses + expenseDTO.Amount > budget.Amount)
            {
                throw new InvalidOperationException("Total expenses exceed the monthly budget.");
            }

            var expense = new Expense
            {
                Amount = expenseDTO.Amount,
                Category = expenseDTO.Category,
                Description = expenseDTO.Description,
                UserId = userId,
                BudgetId = budget.Id,
                Date = DateTime.UtcNow
            };

            await _expenseRepository.AddExpenseAsync(expense);

            return "Expense added successfully.";
        }

public async Task<ExpensePaginationResponseDTO> GetExpensesAsync(ExpenseRequestDTO requestDTO, string userId)
{
    var query = _expenseRepository.GetExpensesQueryable(userId);

    if (!string.IsNullOrEmpty(requestDTO.Category))
    {
        query = query.Where(e => e.Category != null && e.Category.Contains(requestDTO.Category));
    }

    if (requestDTO.Date.HasValue)
    {
        query = query.Where(e => e.Date.Date == requestDTO.Date.Value.Date);
    }

    var totalItems = await query.CountAsync();
    var totalPages = (int)Math.Ceiling((double)totalItems / requestDTO.PageSize);

    var expenses = await query
        .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
        .Take(requestDTO.PageSize)
        .ToListAsync();

    var expenseResponse = expenses.Select(e => new ExpenseResponseDTO
    {
        Amount = e.Amount,
        Category = e.Category,
        Description = e.Description,
        Date = e.Date,
        Id = e.Id
    }).ToList();

    return new ExpensePaginationResponseDTO
    {
        Expenses = expenseResponse,
        TotalItems = totalItems,
        TotalPages = totalPages,
        CurrentPage = requestDTO.PageNumber,
        HasNextPage = requestDTO.PageNumber < totalPages,
        HasPreviousPage = requestDTO.PageNumber > 1
    };
}

        public async Task DeleteExpenseAsync(int id, string userId)
        {
            await _expenseRepository.DeleteExpenseAsync(id, userId);
        }

public async Task<IEnumerable<ExpenseCategoryAmountDTO>> GetExpensesCategoryAmountsAsync(ExpenseRequestCategoryAmountDTO requestDTO, string userId)
{
    var query = _expenseRepository.GetExpensesQueryable(userId);

    if (!string.IsNullOrEmpty(requestDTO.Category))
    {
        query = query.Where(e => e.Category == requestDTO.Category);
    }

    if (requestDTO.StartDate.HasValue)
    {
        query = query.Where(e => e.Date >= requestDTO.StartDate.Value);
    }

    if (requestDTO.EndDate.HasValue)
    {
        query = query.Where(e => e.Date <= requestDTO.EndDate.Value);
    }

    var result = await query
        .Select(e => new ExpenseCategoryAmountDTO
        {
            Category = e.Category,
            TotalAmount = e.Amount ,
            Date =  e.Date
        })
        .ToListAsync();

    return result;
}



    }
}
