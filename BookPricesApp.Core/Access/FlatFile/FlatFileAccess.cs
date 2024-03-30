using BookPricesApp.Core.Access.FlatFile.Models;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Config;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Utils;
using System.Data;

namespace BookPricesApp.Core.Access.FlatFile;

public interface IFlatFileAccess
{
    Option SaveAppConfig();
    Option<AppConfig> GetAppConfig(string? path = null);
    Option CreateNewOutput(string path);
    Option OutputAppend(DataTable books);
    Option LookupAppend(List<AmazonLookup> books);
    Option<List<AmazonLookup>> GetLookupListFor(BookExchange amazon);
}

public class FlatFileAccess : IFlatFileAccess
{
    private OutputFile _outputFile = new();
    private ConfigFile _configFile = new();
    private AmazonLookupFile _amazonLookup;

    public FlatFileAccess(EventBus bus)
    {
        _amazonLookup = new AmazonLookupFile();
    }

    public Option SaveAppConfig() =>
        _configFile.SaveAppConfig();

    public Option<AppConfig> GetAppConfig(string? path = null) =>
        _configFile.GetAppConfig(path);

    public Option CreateNewOutput(string path) =>
        _outputFile.CreateNewExport(path);

    public Option OutputAppend(DataTable books) =>
        _outputFile.Append(books);

    public Option LookupAppend(List<AmazonLookup> books) =>
        _amazonLookup.Append(books);

    public Option<List<AmazonLookup>> GetLookupListFor(BookExchange exchange)
    {
        switch (exchange)
        {
            case BookExchange.Amazon:
                return _amazonLookup.GetLookupList();
            case BookExchange.Ebay:
                {
                    var message = "Ebay Lookups not created yet.";
                    var ex = new Exception(message);
                    return new Option<List<AmazonLookup>>(ex);
                }
            case BookExchange.Unknown:
                {
                    var message = "FlatFileAccess.GetLookupListFor 'Uknown' Selected";
                    var ex = new Exception(message);
                    return new Option<List<AmazonLookup>>(ex);
                }
            default:
                {
                    var message = "FlatFileAccess.GetLookupListFor 'default' Selected";
                    var ex = new Exception(message);
                    return new Option<List<AmazonLookup>>(ex);
                }
        }
    }
}