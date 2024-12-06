using ExpenseTrackerAPI.Models;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Repositories
{
 public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
      Task<User> AddUserAsync(User user);
}

}
