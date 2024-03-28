using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Manager.App;

public interface IAppManager
{
    void SubmitMainEvent(BookExchange exchange);
}

public class AppManager : IAppManager
{
    public AmazonEngine _amazonEngine { get; set; }
    public AppManager(AmazonEngine amazonEngine)
    {
        _amazonEngine = amazonEngine;
    }
    public void SubmitMainEvent(BookExchange exchange)
    {
        switch (exchange)
        {
            case BookExchange.Amazon:
                _amazonEngine.SubmitMainEvent();
                break;
            case BookExchange.Ebay:
                break;
            default:
                break;
        }
    }
}
