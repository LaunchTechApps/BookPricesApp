namespace BookPricesApp.Engine.Models;
public class EventInterval
{
    private DateTime _lastInterval = DateTime.MinValue;
    private int _millisecondThreshold = 750;

    public bool CanProceed()
    {
        var time = _millisecondThreshold * -1;
        var threshold = DateTime.Now.AddMilliseconds(time);

        if (threshold < _lastInterval)
        {
            return false;
        }
        _lastInterval = DateTime.Now;
        return true;
    }
}
