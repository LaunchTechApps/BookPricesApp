using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Text.RegularExpressions;

namespace BookPricesApp.Core.Utils;
public static class Extentions
{
    public static string ToMessageTrace(this Exception ex)
    {
        return $"{ex.Message}\n{ex.StackTrace}";
    }

    public static string CleanOrZero(this string input)
    {
        try
        {
            if (input.ToLower().Contains("free"))
            {
                return "0.00";
            }
            var result = Regex.Matches(input, @"([\d.]+)")
                .ToList().FirstOrDefault()?
                .ToString().Trim()
                .Replace("$", "") ?? "0.00";
            return Convert.ToDouble(result).ToString("0.00");
        }
        catch (Exception)
        {
            return "0.00";
        }
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
