using BookPricesApp.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Manager.Display.Events;

public class EventBase
{
    public BookExchange Exchange { get; set; }
}
public class ProgressEvent : EventBase
{
    public int Count { get; set; }
}

public class StartEvent : EventBase { }

public class StopEvent : EventBase { }