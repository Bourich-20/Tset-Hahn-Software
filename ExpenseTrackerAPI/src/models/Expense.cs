using System;

namespace ExpenseTrackerAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Category { get; set; }  //  Food, Transport, Entertainment

        public string? UserId { get; set; }  
        public User? User { get; set; }  

        public int BudgetId { get; set; }
        public Budget? Budget { get; set; }
    }
}
