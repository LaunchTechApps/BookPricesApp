using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.GUI.Utils;
public static class Extension
{
    public static void Call<TControlType>(this TControlType control, Action<TControlType> del)
        where TControlType : Control
    {
        if (control.InvokeRequired)
        {
            control.Invoke(new Action(() => del(control)));
        }
        else
        {
            del(control);
        }
    }
}