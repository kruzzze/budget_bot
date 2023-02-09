using System.Text.RegularExpressions;
using BudgetBot.Commands;
using BudgetBot.TelegramCommands;
using MediatR;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BudgetBot;

public partial class TelegramBotHostedService : BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IMediator _mediator;
    private readonly Regex _regexp = MyRegex();

    public TelegramBotHostedService(ITelegramBotClient telegramBotClient, IMediator mediator)
    {
        _telegramBotClient = telegramBotClient;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ReceiverOptions receiverOptions = new ()
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
        };
        
        var description = Enum.GetValues(typeof(TelegramCommand))
            .Cast<TelegramCommand>()
            .Select(e => 
            
                new BotCommand()
                {
                    Command = e.GetEnumMemberValue() ?? "Unknown",
                    Description = e.GetDescription() ?? "Unknown"
                }
            ).ToArray();
        
        await _telegramBotClient.SetMyCommandsAsync(
            description,
            cancellationToken: stoppingToken);

        _telegramBotClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken
        );
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        try
        {

            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;

            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;
            var result = new HandlerResult();
            var messageType = message.Entities?.FirstOrDefault(e => e.Type == MessageEntityType.BotCommand);
            if (messageType is null)
            {
                result = await _mediator.Send(new HelpCommand(), cancellationToken);
            }
            else
            {
                var match = _regexp.Match(messageText);
                var command =
                    CommandsFactory.GetCommand(match.Groups.Count > 1 ? match.Groups[1].Value : match.Groups[0].Value);
                
                var messageData = match.Groups.OfType<Group>()
                    .Where(s => !s.ValueSpan.IsEmpty)
                    .Select(s => s.Value)
                    .Skip(1).ToArray();
                
                result = command switch
                {
                    TelegramCommand.Spent => await _mediator.Send(new SpentRequest(messageData), cancellationToken),
                    TelegramCommand.SetBudget => await _mediator.Send(new SetBudgetCommand(messageData), cancellationToken),
                    TelegramCommand.Start => await _mediator.Send(new StartQuery(), cancellationToken),
                    TelegramCommand.Help => await _mediator.Send(new HelpCommand(), cancellationToken),
                    _ => result
                };
            }

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: result.Message,
                disableNotification: true,
                replyToMessageId: update.Message.MessageId,
                replyMarkup: result.ButtonsMarkup,
                cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine("Only for development:" + e);
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    [GeneratedRegex("^.*(/[a-zA-Z]*){1}\\s?([a-zA-Zа-яА-Я]*){0,1}\\s?(\\d*)(\\s[a-zA-Zа-яА-Я]*){0,2}.*")]
    private static partial Regex MyRegex();
}

