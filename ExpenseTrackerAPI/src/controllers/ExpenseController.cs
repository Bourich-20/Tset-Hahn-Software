//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerAPI.DTO;
using ExpenseTrackerAPI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
        }

       [HttpPost]
public async Task<IActionResult> AddExpense([FromBody] ExpenseDTO expenseDTO)
{
  var userIdAlternative = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;


    if (string.IsNullOrEmpty(userIdAlternative))
    {
        return Unauthorized(new { Message = "User not authorized." });
    }

    try
    {
        var result = await _expenseService.AddExpenseAsync(expenseDTO, "1");
        return Ok(new { Message = result });
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new { Message = ex.Message });
    }
}


        [HttpGet][HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value; // Corrected to extract userId from claims
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { Message = "User not authorized." });
                }

                var expenses = await _expenseService.GetExpensesAsync(userId);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Delete an expense
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value; // Corrected to extract userId from claims
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { Message = "User not authorized." });
                }

                await _expenseService.DeleteExpenseAsync(id, userId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
