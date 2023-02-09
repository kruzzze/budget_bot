namespace BudgetBot.Models;

public class CategoryBudget
{
    public Guid Id { get; set; }

    public string Category { get; set; }
    
    public decimal Sum { get; set; }
    
    public IList<Spent> Spents { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}