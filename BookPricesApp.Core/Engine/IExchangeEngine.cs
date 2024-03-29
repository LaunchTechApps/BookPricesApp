using BookPricesApp.Core.Utils;
using System.Data;

namespace BookPricesApp.Core.Engine;
public interface IExchangeEngine
{
    Option Run(List<string> isbnList);
}

public class EngineProvider
{
    public AmazonEngine Amazon { get; private set; }
    public EngineProvider(AmazonEngine amazon)
    {
        Amazon = amazon;
    }
}