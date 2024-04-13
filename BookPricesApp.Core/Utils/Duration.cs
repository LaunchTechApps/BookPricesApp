using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Utils;
internal class Duration
{
    public static int Seconds(int seconds)
    {
        return seconds * 1000;
    }

    public static int Minutes(int minutes)
    {
        return Seconds(60) * minutes;
    }

    public static int Hours(int hours)
    {
        return Minutes(60) * hours;
    }
}