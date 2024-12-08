using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.DTO;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Repositories
{
    public interface IBudgetRepository
{
    Task<Budget?> GetActiveBudgetForUserAsync(string userId);
    Task<ResponseBudgetRequest> AddBudgetAsync(AddBudgetRequest budget ,string userId);
    Task<Budget?> GetBudgetByIdAsync(int budgetId); 
    Task<ResponseBudgetRequest> UpdateBudgetAsync(int budgetId,EditBudgetRequest budget);
    Task<string> DeleteBudgetAsync(int budgetId);
    Task<List<Budget>> GetBudgetsByUserIdAsync(string userId);
}

}
