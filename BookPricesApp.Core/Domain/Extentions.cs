using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Domain;
public static class StringExtensions
{
    public static string ToAmnzFormat(this string str)
    {
        string inputFormat = "ddd, dd MMM yyyy HH:mm:ss 'GMT'";
        string outputFormat = "yyyyMMddTHHmmssZ";
        var culture = System.Globalization.CultureInfo.InvariantCulture;
        DateTime date = DateTime.ParseExact(str, inputFormat, culture);
        return date.ToString(outputFormat);
    }

    public static DateTime IsoToDateTime(this string iso)
    {
        try
        {
            string format = "yyyy-MM-ddTHH:mm:ss.fffZ";
            var culture = CultureInfo.InvariantCulture;
            var style = DateTimeStyles.AdjustToUniversal;
            var result = DateTime.ParseExact(iso, format, culture, style);
            return result;
        }
        catch (Exception)
        {
            return Convert.ToDateTime(iso);
        }
    }
}

public static class DateTimeExtensions
{
    public static string ToIso8601(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }

    public static string ToUnixSecondsStr(this DateTime dateTime)
    {
        var offset = new DateTimeOffset(dateTime);
        return offset.ToUnixTimeSeconds().ToString();
    }

    public static long ToUnixSeconds(this DateTime dateTime)
    {
        var offset = new DateTimeOffset(dateTime);
        return offset.ToUnixTimeSeconds();
    }
}

public static class ExceptionExtensions
{
    public static string ToErrorMessage(this Exception ex)
    {
        return $"MESSAGE:{ex.Message}\nSTACKTRACE:${ex.StackTrace}";
    }
}
