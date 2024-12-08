namespace ExpenseTrackerAPI.DTO
{
    public class ExpenseCategoryAmountDTO
    {
        public string? Category { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }

    }
}
