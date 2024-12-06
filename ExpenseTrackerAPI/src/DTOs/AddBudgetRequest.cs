namespace ExpenseTrackerAPI.DTO
{
    public class AddBudgetRequest
    {
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
