namespace BudgetBot.Models;

public class Budget
{
    public Guid Id { get; set; }

    public decimal Sum { get; set; }
    
    public UsersGroup UsersGroup { get; set; }

    public CategoryBudget[] CategoryBudgets { get; set; }
}