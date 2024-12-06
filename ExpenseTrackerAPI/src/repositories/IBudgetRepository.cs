using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.DTO;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Repositories
{
    public interface IBudgetRepository
{
    Task<Budget?> GetActiveBudgetForUserAsync(string userId);
    Task<ResponseBudgetRequest> AddBudgetAsync(AddBudgetRequest budget);
    Task<Budget?> GetBudgetByIdAsync(int budgetId);  // Budget? au lieu de Budget
    Task<ResponseBudgetRequest> UpdateBudgetAsync(EditBudgetRequest budget);
    Task<string> DeleteBudgetAsync(int budgetId);
}

}
