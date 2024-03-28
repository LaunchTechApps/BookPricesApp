using BookPricesApp.Core.Domain.Types;

namespace BookPricesApp.Core.Domain.Events;
public class EventBase
{
    public BookExchange Exchange { get; set; } = BookExchange.Unknown;
}
public class ProgressEvent : EventBase
{
    public int Count { get; set; }
}

public class StartEvent : EventBase { }

public class StopEvent : EventBase { }