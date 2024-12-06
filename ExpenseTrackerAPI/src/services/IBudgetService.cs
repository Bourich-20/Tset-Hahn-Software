using ExpenseTrackerAPI.DTO;
using ExpenseTrackerAPI.Models;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Services
{
    public interface IBudgetService
    {
        Task<ResponseBudgetRequest> AddBudgetAsync(AddBudgetRequest budgetRequest);
        Task<ResponseBudgetRequest> UpdateBudgetAsync(int budgetId, EditBudgetRequest budgetRequest);
        Task<Budget> GetBudgetByIdAsync(int budgetId);
        Task<Budget> GetActiveBudgetForUserAsync(string userId);
        Task<string> DeleteBudgetAsync(int budgetId);
    }
}
