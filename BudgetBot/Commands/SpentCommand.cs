using BudgetBot.TelegramCommands;
using Telegram.Bot.Types.ReplyMarkups;

namespace BudgetBot.Commands;

public class SpentRequest : MediatR.IRequest<HandlerResult>
{
    public string Category { get; } = string.Empty;
    
    public decimal Sum { get; }

    public string? AdditionalInfo { get; }

    public string Error { get; } = string.Empty;
    
    public SpentRequest(string[] message)
    {
        if (message.Length < 3)
        {
            Error = "Command is incorrect. Please enter all required parameters.";
            return;
        }
        
        if (!string.IsNullOrWhiteSpace(message[1]))
            Category = message[1];
        else
            Error = "Please enter Category. ";

        if (decimal.TryParse(message[2], out var sum))
            Sum = sum;
        else
            Error += "Please enter Sum.";
        
        AdditionalInfo = string.Join("", message[3..]);
    }

    public class SpentCommandHandler : MediatR.IRequestHandler<SpentRequest, HandlerResult>
    {

        public Task<HandlerResult> Handle(SpentRequest request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Error))
                return Task.FromResult<HandlerResult>(new HandlerResult()
                {
                    Message = request.Error,
                    ButtonsMarkup = new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Help",
                            TelegramCommand.Help.GetEnumMemberValue() ?? "Help"))
                });


            return Task.FromResult(new HandlerResult()
            {
                Message = $"You entered:\n Command: Spent\n " +
                          $"Category: {request.Category}\n " +
                          $"Spent: {request.Sum}\n" +
                          $"Additional information: {request.AdditionalInfo}"
            });
        }
    }
    
}

