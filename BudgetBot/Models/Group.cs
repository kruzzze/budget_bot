namespace BudgetBot.Models;

public class UsersGroup
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public User[] Users { get; set; }
}