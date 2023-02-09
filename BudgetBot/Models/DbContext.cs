using Microsoft.EntityFrameworkCore;

namespace BudgetBot.Models;

public class BudgetContext : DbContext
{
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<UsersGroup> Groups { get; set; }
    public DbSet<Spent> Spent { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<CategoryBudget> CategoryBudget { get; set; }
    
    public BudgetContext(DbContextOptions<BudgetContext> options)
        : base(options)
    {
    }
    
    
}