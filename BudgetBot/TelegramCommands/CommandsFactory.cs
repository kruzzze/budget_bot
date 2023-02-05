namespace BudgetBot.TelegramCommands;

public static class CommandsFactory
{
    public static TelegramCommand GetCommand(string messageCommand)
    {
        switch (messageCommand.ToLowerInvariant())
        {
            case "/start": return TelegramCommand.Start;
            case "/spent": return TelegramCommand.Spent;
            case "/help": return TelegramCommand.Help;
            case "/setbudget": return TelegramCommand.SetBudget;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}