
namespace ExpenseTrackerAPI.DTO;

public class ExpenseResponseDTO
{
    public decimal Amount { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
 public int Id { get; set; }  }
