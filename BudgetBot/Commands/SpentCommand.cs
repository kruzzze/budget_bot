using BudgetBot.Models;
using BudgetBot.TelegramCommands;
using Microsoft.EntityFrameworkCore;
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
            Category = message[1].ToLowerInvariant().Trim();
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

        private readonly BudgetContext _budgetContext;

        public SpentCommandHandler(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
        }

        public async Task<HandlerResult> Handle(SpentRequest request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Error))
                return new HandlerResult()
                {
                    Message = request.Error,
                    ButtonsMarkup = new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Help",
                            TelegramCommand.Help.GetEnumMemberValue() ?? "Help"))
                };

            var category = await _budgetContext.CategoryBudget.FirstOrDefaultAsync(cb => cb.Category == request.Category,
                cancellationToken: cancellationToken);
            
            if (category is null)
                await _budgetContext.CategoryBudget.AddAsync(new CategoryBudget()
                {
                    Category = request.Category,
                    Id = Guid.NewGuid(),
                    UpdatedAt = DateTime.Now,
                    Spents = new List<Spent>(){
                        new Spent()
                        {
                            AdditionalInfo = request.AdditionalInfo,
                            DateTime = DateTime.Now,
                            Id = Guid.NewGuid(),
                            Sum = request.Sum,
                        } 
                    },
                    Sum = request.Sum
                }, cancellationToken);
            else
            {
                category.Spents.Add(
                    new Spent()
                    {
                        AdditionalInfo = request.AdditionalInfo,
                        DateTime = DateTime.Now,
                        Id = Guid.NewGuid(),
                        Sum = request.Sum,
                    });
                category.Sum += request.Sum;
                category.UpdatedAt = DateTime.Now;
            }

            await _budgetContext.SaveChangesAsync(cancellationToken);


            var res = _budgetContext.CategoryBudget.ToList();


            return new HandlerResult()
            {
                Message = $"You entered:\n Command: Spent\n " +
                          $"Category: {request.Category}\n " +
                          $"Spent: {request.Sum}\n" +
                          $"Additional information: {request.AdditionalInfo}"
            };
        }
    }
    
}

