using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Data
{
    public class ExpenseTrackerDbContext : IdentityDbContext<User>
    {
        public required DbSet<Budget> Budgets { get; set; }
        public required DbSet<Expense> Expenses { get; set; }  // Add DbSet for Expenses

        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
