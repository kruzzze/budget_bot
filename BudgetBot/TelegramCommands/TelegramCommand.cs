using System.ComponentModel;
using System.Runtime.Serialization;

namespace BudgetBot.TelegramCommands;

public enum TelegramCommand
{
    [EnumMember(Value = "/start")]
    [Description("Start using this dazzling bot")]
    Start,
    
    [EnumMember(Value = "/spent")]
    [Description(@"Set your current spent. Format: /spent category sum additionalInfo")]
    Spent,
    
    [EnumMember(Value = "/setbudget")]
    [Description("Set your budget for a period. Format: /setbudget category sum period")]
    SetBudget,
    
    [EnumMember(Value = "/help")]
    [Description("Show help")]
    Help
}