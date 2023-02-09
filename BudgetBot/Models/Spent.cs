namespace BudgetBot.Models;

public class Spent
{
    public Guid Id { get; set; }

    public decimal Sum { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public string AdditionalInfo { get; set; }
}