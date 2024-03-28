using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine.Models;
using BookPricesApp.Core.Utils;

namespace BookPricesApp.Core.Engine;
public class AmazonEngine
{
    private bool _running = false;
    private EventBus _bus;
    private EventInterval _interval = new EventInterval();
    public AmazonEngine(EventBus bus)
    {
        _bus = bus;
    }
    public void SubmitMainEvent()
    {
        if (!_interval.CanProceed()) return;

        if (!_running)
        {
            start();
            return;
        }

        if (_running)
        {
            stop();
            return;
        }
    }

    private void start()
    {
        _bus.Publish(new StartEvent { Exchange = BookExchange.Amazon });
        new Thread(progress).Start();
        _running = true;
    }

    private void stop()
    {
        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _running = false;
    }

    private void progress()
    {
        var count = 0;
        var max = 100;
        while (count < max)
        {
            if (!_running) break;

            count++;
            Thread.Sleep(50);

            if (!_running) break;

            _bus.Publish(new ProgressEvent
            {
                Count = count,
                Exchange = BookExchange.Amazon
            });
        }

        Thread.Sleep(750);
        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _running = false;
    }
}