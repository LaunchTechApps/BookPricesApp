using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Utils;
public class ProgressCounter
{
    private int _maxCount { get; set; }
    private int _count { get; set; } = 0;
    public int Percent 
    { 
        get 
        { 
            if (_maxCount <= 0)
            {
                return 100;
            }
            if (_count == 0)
            {
                return 0;
            }
            if (_count > _maxCount)
            {
                return 100;
            }
            return _count * 100 / _maxCount; 
        } 
    }

    public ProgressCounter(int maxCount)
    {
        _maxCount = maxCount;
    }

    public int Increment() 
    {
        _count++;
        return Percent;
    }
}