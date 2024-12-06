using ExpenseTrackerAPI.Models;
using System.Threading.Tasks;
using ExpenseTrackerAPI.DTO;

namespace ExpenseTrackerAPI.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterRequest registerRequest);
    Task<string> LoginAsync(LoginRequest loginRequest);

    }
}
