namespace ExpenseTrackerAPI.DTO
{
    public class ExpenseRequestCategoryAmountDTO
    {
        public string? Category { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
