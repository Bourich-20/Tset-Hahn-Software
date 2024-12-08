namespace ExpenseTrackerAPI.DTO;

public class ExpenseRequestDTO
{
    public int PageNumber { get; set; } = 1;  
    public int PageSize { get; set; } = 10;   
    public string? Category { get; set; }     
    public DateTime? Date { get; set; }       
}
