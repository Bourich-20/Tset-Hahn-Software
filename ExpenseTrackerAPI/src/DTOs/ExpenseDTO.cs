namespace ExpenseTrackerAPI.DTO
{
    public class ExpenseDTO
    {
        public decimal Amount { get; set; }
        public string? Category { get; set; }
        public int BudgetId { get; set; }
        public string? Description { get; set; }  // Add Description to DTO
    }
}
