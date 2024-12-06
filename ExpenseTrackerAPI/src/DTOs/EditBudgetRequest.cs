namespace ExpenseTrackerAPI.DTO
{
    public class EditBudgetRequest
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
