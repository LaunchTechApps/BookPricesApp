using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine.Contract;
using BookPricesApp.Core.Utils;

namespace BookPricesApp.Core.Engine;

public class EngineProvider
{
    public AmazonEngine Amazon { get; private set; }
    public EbayEngine Ebay { get; private set; }
    public EngineProvider(AmazonEngine amazon, EbayEngine ebayEngine)
    {
        Amazon = amazon;
        Ebay = ebayEngine;
    }

    public TResult<IExchangeEngine> GetExchange(BookExchange exchange)
    {
        switch (exchange)
        {
            case BookExchange.Amazon: return Amazon;
            case BookExchange.Ebay: return Ebay;
            default: return Error.Create("EngineProvider.GetExchange", "Uknonw Exchange");
        }
    }
}