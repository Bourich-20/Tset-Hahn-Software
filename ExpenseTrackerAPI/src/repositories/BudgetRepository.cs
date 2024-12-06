using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.DTO;

namespace ExpenseTrackerAPI.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public BudgetRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        // Récupérer le budget actif d'un utilisateur
        public async Task<Budget?> GetActiveBudgetForUserAsync(string userId)
        {
            return await _context.Budgets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.StartDate <= DateTime.UtcNow && b.EndDate >= DateTime.UtcNow);
        }

        // Ajouter un budget
        public async Task<ResponseBudgetRequest> AddBudgetAsync(AddBudgetRequest budgetRequest)
        {
            var budget = new Budget
            {
                Amount = budgetRequest.Amount,
                StartDate = budgetRequest.StartDate,
                EndDate = budgetRequest.EndDate
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return new ResponseBudgetRequest
            {
                Id = budget.Id,
                Amount = budget.Amount,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate,
                Message = "Budget added successfully"
            };
        }

        // Récupérer un budget par son ID
    public async Task<Budget?> GetBudgetByIdAsync(int budgetId)
{
    return await _context.Budgets.FindAsync(budgetId);
}

        // Mettre à jour un budget
public async Task<ResponseBudgetRequest> UpdateBudgetAsync(EditBudgetRequest budgetRequest)
{
    var budget = await _context.Budgets.FindAsync(budgetRequest.Id);
    if (budget == null)
    {
        return new ResponseBudgetRequest { Message = "Budget not found" };
    }

    budget.Amount = budgetRequest.Amount;
    budget.StartDate = budgetRequest.StartDate;
    budget.EndDate = budgetRequest.EndDate;

    _context.Budgets.Update(budget);
    await _context.SaveChangesAsync();

    return new ResponseBudgetRequest
    {
        Id = budget.Id,
        Amount = budget.Amount,
        StartDate = budget.StartDate,
        EndDate = budget.EndDate,
        Message = "Budget updated successfully"
    };
}


        // Supprimer un budget
        public async Task<string> DeleteBudgetAsync(int budgetId)
        {
            var budget = await _context.Budgets.FindAsync(budgetId);
            if (budget == null)
            {
                return "Budget not found";
            }

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return "Budget deleted successfully";
        }
    }
}
