using BudgetBot.TelegramCommands;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;

namespace BudgetBot.Commands;

public class StartQuery : MediatR.IRequest<HandlerResult>
{
    public class StartQueryHandler : IRequestHandler<StartQuery, HandlerResult>
    {
        public Task<HandlerResult> Handle(StartQuery request, CancellationToken cancellationToken)
        {
            var helpCommand = TelegramCommand.Help.GetEnumMemberValue();
            
            var result = new HandlerResult()
            {
                Message =
                    $"Glad to see you! Please explore {helpCommand} to start using bot.\n" +
                    $"If you want to learn how to build your own budget - explore How To",
                ButtonsMarkup = new InlineKeyboardMarkup(
                    new []
                    {
                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(TelegramCommand.Help.ToString(), helpCommand),
                        InlineKeyboardButton.WithUrl("How to build your own budget", "https://t.me/bitkogan_prosto/98")
                    }
                )
            };
            
            return Task.FromResult(result);
        }
    }
}