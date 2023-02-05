using System.Reflection;
using BudgetBot;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

var token = Environment.GetEnvironmentVariable("TelegramBotToken")
            ?? throw new Exception("Telegram Bot Token is required");

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(token));

        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddHostedService<TelegramBotHostedService>();
    })
    .Build()
    .RunAsync();
