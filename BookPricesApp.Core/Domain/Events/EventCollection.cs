using BookPricesApp.Core.Domain.Types;

namespace BookPricesApp.Core.Domain.Events;
public class ExchangeEvent
{
    public BookExchange Exchange { get; set; } = BookExchange.Unknown;
}
public class ProgressEvent : ExchangeEvent
{
    public int Count { get; set; }
}

public class StartEvent : ExchangeEvent { }

public class StopEvent : ExchangeEvent { }

public class StopRequestEvent : ExchangeEvent { }

public class AlertEvent 
{ 
    public string Message { get; set; } = string.Empty;
    public AlertEvent()
    {
        
    }

    public AlertEvent(string message)
    {
        Message = message;
    }
}

public class ErrorEvent
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}