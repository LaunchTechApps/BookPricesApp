using System;
using System.Collections.Generic;
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
}
