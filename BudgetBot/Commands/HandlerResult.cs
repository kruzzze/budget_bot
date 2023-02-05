using Telegram.Bot.Types.ReplyMarkups;

namespace BudgetBot.Commands;

public class HandlerResult
{
    public string Message { get; set; } = string.Empty;
    
    public InlineKeyboardMarkup? ButtonsMarkup { get; set; }
}