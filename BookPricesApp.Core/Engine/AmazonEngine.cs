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

    public Option Run(List<string> isbnList)
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
            return new(ex);
        }
        return new();
    }

    private async Task run(List<string> isbnList)
    {
        _bus.Publish(new StartEvent { Exchange = BookExchange.Amazon });

        var lookupResult = _db.GetAmazonLookup();
        if (lookupResult.Ex != null)
        {
            publishErrorAndStop(lookupResult.Ex);
            return;
        }
        if (lookupResult.Data == null)
        {
            lookupResult.Data = new List<AmazonLookup>();
        }

        var noLookup = new List<string>();
        {
            var lookupDictionary = lookupResult.Data
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

        if (newLookupsResult.Ex != null)
        {
            publishErrorAndStop(newLookupsResult.Ex);
            return;
        }
        if (newLookupsResult.Data == null)
        {
            newLookupsResult.Data = new List<AmazonLookup>();
        }
        else
        {
            _db.InsertAmazonLookup(newLookupsResult.Data);
        }

        var combined = new List<AmazonLookup>();
        combined.AddRange(lookupResult.Data);
        combined.AddRange(newLookupsResult.Data);

        // need to save the combined to the lookup file

        var outputList = await _amazonAccess.GetDataByLookup(combined);

        if (outputList.Ex != null) 
        {
            // what do we do here?
        }
        else if (outputList.Data != null)
        {
            // these should return errors if they have any
            _db.ExecuteQuery("DROP TABLE IF EXISTS [Output]");
            _db.ExecuteQuery(Tables.CreateOutputTable);
            _db.InsertAmazonOutput(outputList.Data);
        }
        
        //if (outputList.Ex != null)
        //{
        //    publishErrorAndStop(outputList.Ex);
        //    return;
        //}


        var booksDataTable = new DataTable();
        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _engineState = EngineState.NotRunning;
    }

    private void publishErrorAndStop(Exception ex)
    {
        _bus.Publish(new ErrorEvent { Message = ex.ToErrorMessage() });
        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _engineState = EngineState.NotRunning;
    }

    private void progress()
    {
        var count = 0;
        var max = 100;
        while (count < max)
        {
            if (_engineState == EngineState.NotRunning || _engineState == EngineState.Stopping) 
            {
                break; 
            }

            count++;
            Thread.Sleep(50);

            if (_engineState == EngineState.NotRunning || _engineState == EngineState.Stopping)
            {
                break;
            }

            _bus.Publish(new ProgressEvent
            {
                Percent = count,
                Exchange = BookExchange.Amazon
            });
        }

        Thread.Sleep(2750);
        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _engineState = EngineState.NotRunning;
    }
}