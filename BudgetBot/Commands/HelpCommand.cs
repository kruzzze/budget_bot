using BudgetBot.TelegramCommands;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;

namespace BudgetBot.Commands;

public class HelpCommand : IRequest<HandlerResult>
{
    
    public class HelpCommandHandler : IRequestHandler<HelpCommand, HandlerResult>
    {
        public Task<HandlerResult> Handle(HelpCommand request, CancellationToken cancellationToken)
        {
            var description = Enum.GetValues(typeof(TelegramCommand))
                .Cast<TelegramCommand>()
                .Select(e => 
                (
                    KeyboardButton: InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(text: e.ToString(),
                        e.GetEnumMemberValue() ?? e.ToString()),
                    Message: $"{e.GetEnumMemberValue()} {e.GetDescription()}"
                )).ToArray();

            var text = "Please use command from list:\n" + 
                       string.Join("\n", description.Select(d => d.Message).ToArray());

            var buttons =  new InlineKeyboardMarkup(
                description.Select(s => s.KeyboardButton));
            
            return Task.FromResult<HandlerResult>(
                new HandlerResult()
                {
                    Message = text,
                    ButtonsMarkup = buttons
                });
            
        }
    }
    
}