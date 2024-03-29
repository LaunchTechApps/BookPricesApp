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
    Option LookupAppend(List<BookLookup> books);
    Option<List<BookLookup>> GetLookupListFor(BookExchange amazon);
}

public class FlatFileAccess : IFlatFileAccess
{
    private string _lookupPath => $"{Directory.GetCurrentDirectory()}\\Domain\\Files\\BookLookup.csv";

    private OutputFile _outputFile = new();
    private ConfigFile _configFile = new();
    private LookupFile _lookup;

    public FlatFileAccess(EventBus bus)
    {
        _lookup = new LookupFile(_lookupPath);
    }

    public Option SaveAppConfig() =>
        _configFile.SaveAppConfig();

    public Option<AppConfig> GetAppConfig(string? path = null) => 
        _configFile.GetAppConfig(path);

    public Option CreateNewOutput(string path) => 
        _outputFile.CreateNewExport(path);

    public Option OutputAppend(DataTable books) => 
        _outputFile.Append(books);

    public Option LookupAppend(List<BookLookup> books) => 
        _lookup.Append(books);

    public Option<List<BookLookup>> GetLookupListFor(BookExchange exchange) => 
        _lookup.GetLookupListFor(exchange);
}
