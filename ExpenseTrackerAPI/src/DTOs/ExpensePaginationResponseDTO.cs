namespace ExpenseTrackerAPI.DTO;

public class ExpensePaginationResponseDTO
{
    public List<ExpenseResponseDTO> Expenses { get; set; } = new List<ExpenseResponseDTO>();
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
