﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.GUI.Utils;
public static class Extension
{
    public static void Invoke<TControlType>(this TControlType control, Action<TControlType> del)
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