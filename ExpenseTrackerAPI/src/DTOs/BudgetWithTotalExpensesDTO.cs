namespace ExpenseTrackerAPI.DTO
{
    public class BudgetWithTotalExpensesDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalExpenses { get; set; }
    }
}
