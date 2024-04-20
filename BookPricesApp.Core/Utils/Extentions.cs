using Microsoft.Extensions.Configuration;

namespace BookPricesApp.Core.Utils;
public static class Extentions
{
    public static string ToMessageTrace(this Exception ex)
    {
        return $"{ex.Message}\n{ex.StackTrace}";
    }

    public static List<T> Plus<T>(this List<T> list, List<T> toAdd)
    {
        var combined = new List<T>();
        combined.AddRange(list);
        combined.AddRange(toAdd);
        return combined;
    }

    public static bool GetBoolean(this IConfiguration config, string key)
    {
        var stringValue = config.GetSection(key).Value;
        bool boolValue;
        if (!bool.TryParse(stringValue, out boolValue))
        {
            return false;
        }
        return boolValue;
    }
}
