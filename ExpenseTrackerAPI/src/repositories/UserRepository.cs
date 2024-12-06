using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public UserRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        // Ajouter un utilisateur à la base de données
        public async Task<User> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Obtenir un utilisateur par email
     public async Task<User?> GetUserByEmailAsync(string email)
{
    if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentException("Email cannot be null or empty.", nameof(email));

    return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
}

    }
}
