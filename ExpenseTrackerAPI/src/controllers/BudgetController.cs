using ExpenseTrackerAPI.DTO;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;


namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpPost]
public async Task<IActionResult> AddBudget([FromBody] AddBudgetRequest budgetRequest)
{
    if (budgetRequest == null)
    {
        return BadRequest("Invalid budget data.");
    }

    try
    {
        var result = await _budgetService.AddBudgetAsync(budgetRequest);
        return Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
    }
}


        [HttpPut("{budgetId}")]
        public async Task<IActionResult> UpdateBudget(int budgetId, [FromBody] EditBudgetRequest budgetRequest)
        {
            if (budgetRequest == null)
            {
                return BadRequest("Invalid budget data.");
            }

            try
            {
                var result = await _budgetService.UpdateBudgetAsync(budgetId, budgetRequest);
                if (result == null)
                {
                    return NotFound(new { Message = "Budget not found." });
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{budgetId}")]
        public async Task<IActionResult> GetBudgetById(int budgetId)
        {
            try
            {
                var budget = await _budgetService.GetBudgetByIdAsync(budgetId);
                if (budget == null)
                {
                    return NotFound(new { Message = "Budget not found." });
                }
                return Ok(budget);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("active/{userId}")]
        public async Task<IActionResult> GetActiveBudgetForUser(string userId)
        {
            try
            {
                var budget = await _budgetService.GetActiveBudgetForUserAsync(userId);
                if (budget == null)
                {
                    return NotFound(new { Message = "No active budget found for this user." });
                }
                return Ok(budget);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{budgetId}")]
        public async Task<IActionResult> DeleteBudget(int budgetId)
        {
            try
            {
                var result = await _budgetService.DeleteBudgetAsync(budgetId);
                return Ok(new { Message = result });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
