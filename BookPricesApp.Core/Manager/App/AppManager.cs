using BookPricesApp.Core.Access;
using BookPricesApp.Core.Access.FlatFile;
using BookPricesApp.Core.Domain.Config;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine;
using BookPricesApp.Core.Utils;

namespace BookPricesApp.Core.Manager.App;

public interface IAppManager
{
    void SubmitMainEvent(BookExchange exchange, string isbnFilePath);
}

public class AppManager : IAppManager
{
    private EngineProvider _engineProvider { get; set; }
    private EventBus _bus;
    private AppConfig _config;
    private IFlatFileAccess _flatFileAccess;
    public AppManager(
        EngineProvider engineProvider, 
        EventBus bus, 
        AppConfig config,
        IFlatFileAccess flatFileAccess)
    {
        _engineProvider = engineProvider;
        _bus = bus;
        _config = config;
        _flatFileAccess = flatFileAccess;
    }
    public void SubmitMainEvent(BookExchange exchange, string isbnFilePath)
    {
        if (!File.Exists(isbnFilePath))
        {
            _bus.Publish(new AlertEvent($"File not found: {isbnFilePath}"));
            return;
        }

        var isbnList = File.ReadAllLines(isbnFilePath)
            .Select(s => s.Trim())
            .ToList();

        var firstISBN = isbnList.FirstOrDefault() ?? "";
        if (!ISBNValidation.IsValidISBN(firstISBN))
        {
            _bus.Publish(new AlertEvent($"Fisrt line was an invalid ISBN: {firstISBN}"));
            return;
        }

        switch (exchange)
        {
            case BookExchange.Amazon:
                _config.Amazon.IsbnFilePath = isbnFilePath;
                _flatFileAccess.SaveAppConfig();
                _engineProvider.Amazon.Run(isbnList);
                break;
            case BookExchange.Ebay:
                //_config.Ebay.IsbnFilePath = isbnFilePath;
                //_flatFileAccess.SaveAppConfig();
                _bus.Publish(new AlertEvent($"Ebay not implemented yet"));
                break;
            default:
                _bus.Publish(new AlertEvent($"Unknown book exchange: {exchange}"));
                break;
        }
    }
}
