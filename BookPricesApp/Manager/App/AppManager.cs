using BookPricesApp.Domain.Types;
using BookPricesApp.Engine;

namespace BookPricesApp.Manager.App;
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
                MessageBox.Show("Coming Soon!");
                break;
            default:
                break;
        }
    }

    public void Stop(BookExchange exchange)
    {
    }
}
