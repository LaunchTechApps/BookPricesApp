using BookPricesApp.Core.Access.Amazon;
using BookPricesApp.Core.Access.FlatFile;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine.Models;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using BookPricesApp.Repo;
using BookPricesApp.Repo.Migrations;
using System.Data;
using System.Linq;

namespace BookPricesApp.Core.Engine;

public enum EngineState
{
    Running,
    NotRunning,
    Stopping
}

public class AmazonEngine : IExchangeEngine
{
    private EventBus _bus;
    private EventInterval _interval = new EventInterval();
    private IFlatFileAccess _flatFileAccess;
    private EngineState _engineState = EngineState.NotRunning;
    private AmazonAccess _amazonAccess;
    private BookPriceRepo _db;

    public AmazonEngine(
        EventBus bus, 
        AmazonAccess amazonAccess, 
        IFlatFileAccess flatFileAccess,
        BookPriceRepo db)
    {
        _bus = bus;
        _amazonAccess = amazonAccess;
        _flatFileAccess = flatFileAccess;
        _db = db;
    }

    public Result<int, Exception> Run(List<string> isbnList)
    {
        try
        {
            if (!_interval.CanProceed()) 
            { 
                return new(); 
            }

            switch (_engineState)
            {
                case EngineState.Running:
                    _engineState = EngineState.Stopping;
                    _bus.Publish(new StopRequestEvent { Exchange = BookExchange.Amazon });
                    break;
                case EngineState.NotRunning:
                    _engineState = EngineState.Running;
                    new Thread(() => _ = run(isbnList)).Start();
                    break;
                case EngineState.Stopping:
                    // TODO: do we need to do anything here?
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            return ex;
        }
        return 0;
    }

    private async Task run(List<string> isbnList)
    {
        _bus.Publish(new StartEvent { Exchange = BookExchange.Amazon });
        publishNewStatus("Getting Amazon Lookups 1/2");

        var lookupResult = _db.GetAmazonLookup();
        if (lookupResult.Error != null)
        {
            publishErrorAndStop(lookupResult.Error);
            return;
        }

        var lookups = lookupResult.Value != null ? 
            lookupResult.Value : new List<AmazonLookup>();
        

        var noLookup = new List<string>();
        {
            var lookupDictionary = lookups
                .Select(l => l.ISBN13)
                .Distinct()
                .ToDictionary(key => key, value => true);

            foreach (var book in isbnList)
            {
                if (!lookupDictionary.ContainsKey(book))
                {
                    noLookup.Add(book);
                }
            }
        }

        await _amazonAccess.SetRefresToken();
        var newLookupsResult = await _amazonAccess.GetLookupsFor(noLookup);

        if (newLookupsResult.Error != null)
        {
            publishErrorAndStop(newLookupsResult.Error);
            return;
        }

        var newLookups = newLookupsResult.Value != null ?
            newLookupsResult.Value : new List<AmazonLookup>();

        if (newLookupsResult.Value == null)
        {
            var msg = "newLookupsResult.Value was found null";
            var ex = new Exception(msg);
            publishErrorAndStop(ex);
            return;
        }

        var combined = new List<AmazonLookup>();
        combined.AddRange(lookups);
        combined.AddRange(newLookupsResult.Value);

        _db.InsertAmazonLookup(newLookupsResult.Value);

        publishNewStatus("Getting Amazon Book Prices 2/2");
        var outputList = await _amazonAccess.GetDataByLookup(combined);

        if (outputList.Error != null) 
        {
            publishErrorAndStop(outputList.Error);
            return;
        }
        else if (outputList.Value != null)
        {
            // TODO: if any of the below methods error, they need to be handled
            _db.ExecuteQuery("DROP TABLE IF EXISTS [Output]");
            _db.ExecuteQuery(Tables.CreateOutputTable);
            _db.InsertAmazonOutput(outputList.Value);
        }
        else if (outputList.Value == null)
        {
            var msg = "outputList.Value was found null and did not error";
            var ex = new Exception(msg);
            publishErrorAndStop(ex);
            return;
        }

        publishNewStatus("Exporting");
        var test = _db.GetExportFor(BookExchange.Amazon);
        if (test.Error != null)
        {
            publishErrorAndStop(test.Error);
            return;
        }
        if (test.Value == null)
        {
            publishErrorAndStop(new Exception("export value was found null"));
            return;
        }

        _flatFileAccess.CreateNewExport();
        var outputResult = _flatFileAccess.OutputAppend(test.Value);
        if (outputResult.Error != null)
        {
            publishErrorAndStop(outputResult.Error);
            return;
        }

        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _engineState = EngineState.NotRunning;

        

        publishNewStatus("Success!");
    }



    private void publishErrorAndStop(Exception ex)
    {
        _bus.Publish(new ErrorEvent { Message = ex.ToErrorMessage() });
        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        publishNewStatus("...");
        _engineState = EngineState.NotRunning;
    }

    private void publishNewStatus(string status)
    {
        _bus.Publish(new StatusLabelChangeEvent 
        { 
            Exchange = BookExchange.Amazon,
            Status = status
        });
    }
}