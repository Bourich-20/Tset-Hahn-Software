using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;  // Ajouté pour l'utilisation de ICollection

namespace ExpenseTrackerAPI.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();

        public User()
        {
            Expenses = new List<Expense>();  
            Budgets = new List<Budget>();    
        }
    }
}
