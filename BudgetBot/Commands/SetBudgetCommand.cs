namespace BudgetBot.Commands;

public class SetBudgetCommand : MediatR.IRequest<HandlerResult>
{
    public string Category { get; } = string.Empty;
    
    public decimal Sum { get; }

    public string Period { get; } = "Month";

    public string Error { get; } = string.Empty;
    
    public SetBudgetCommand(string[] message)
    {
        if (message.Length < 3)
        {
            Error = "Command is incorrect. Please enter all required parameters.";
            return;
        }
        
        Category = message[1];
        if (decimal.TryParse(message[2], out var sum))
            Sum = sum;
        else
        {
            Error = "Command is incorrect. Please enter all required parameters.";
            return;
        }

        if (message.Length > 3)
            Period = message[3];
    }

    public class SetBudgetCommandHandler : MediatR.IRequestHandler<SetBudgetCommand, HandlerResult>
    {
        public Task<HandlerResult> Handle(SetBudgetCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Error))
                return Task.FromResult(new HandlerResult() {Message = request.Error});


            return Task.FromResult(new HandlerResult() {Message = $"You entered:\n Command: Set Budget command\n " +
                                   $"Category: {request.Category}\n " +
                                   $"Sum: {request.Sum}\n" +
                                   $"Period: {request.Period}"} );

        }
    }
}