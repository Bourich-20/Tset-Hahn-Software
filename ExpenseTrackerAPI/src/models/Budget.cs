using System;
using System.Collections.Generic;

namespace ExpenseTrackerAPI.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public decimal Amount { get; set; } // Monthly budget limit
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? UserId { get; set; }  
        public User? User { get; set; }  
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
