using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Utils;
public class EventBus
{
    private readonly Dictionary<Type, Action<object>> _eventHandlers = new();

    public void OnEvent<T>(Action<T> handler)
    {
        var eventType = typeof(T);
        if (!_eventHandlers.ContainsKey(eventType))
        {
            _eventHandlers[eventType] = eventData => handler((T)eventData);
        }
        else
        {
            _eventHandlers[eventType] += eventData => handler((T)eventData);
        }
    }

    public void Publish<T>(T eventData)
    {
        if (eventData == null) return;
        var eventType = typeof(T);
        if (_eventHandlers.TryGetValue(eventType, out var handler))
        {
            handler.Invoke(eventData);
        }
    }
}
