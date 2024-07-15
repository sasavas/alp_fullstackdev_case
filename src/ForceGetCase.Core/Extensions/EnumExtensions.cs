using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ForceGetCase.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetField(enumValue.ToString())
            .GetCustomAttribute<DisplayAttribute>();
        
        return displayAttribute?.Name ?? enumValue.ToString();
    }
    
    public static Dictionary<int, string> GetEnumDisplayNames<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .ToDictionary(e => Convert.ToInt32(e), e => e.GetDisplayName());
    }
}
