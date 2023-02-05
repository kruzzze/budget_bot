using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace BudgetBot;

public static class EnumExtensions
{
    public static string? GetEnumMemberValue(this Enum value)
    {
        return value
            .GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<EnumMemberAttribute>()
            ?.Value;
    }

    public static string? GetDescription(this Enum value)
    {
        return value
            .GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DescriptionAttribute>()
            ?.Description;
    }

    public static TEnum ToEnumValue<TEnum>(this string value)
    {
        var result = (TEnum)Enum.Parse(typeof(TEnum), value);

        return result;
    }
}