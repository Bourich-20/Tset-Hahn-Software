namespace ExpenseTrackerAPI.DTO
{
    public class ResponseBudgetRequest
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Message { get; set; }
    }
}
