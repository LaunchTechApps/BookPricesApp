using BookPricesApp.Core.Access.Amazon;
using BookPricesApp.Core.Access.DB;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine.Models;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using System.Data;

namespace BookPricesApp.Core.Engine;

public enum EngineState
{
    Running,
    NotRunning,
    Stopping
}

public class AmazonEngine : IExchangeEngine
{
    private bool _needToStopThread => _engineState == EngineState.Stopping;
    private EventBus _bus;
    private EventInterval _interval = new EventInterval();
    private EngineState _engineState = EngineState.NotRunning;
    private AmazonAccess _amazonAccess;
    private DBAccess _db;

    private Thread? _thread;

    public AmazonEngine(
        EventBus bus, 
        AmazonAccess amazonAccess, 
        DBAccess db)
    {
        _bus = bus;
        _amazonAccess = amazonAccess;
        _db = db;
    }

    public Result<Success, Exception> Run(List<string> isbnList)
    {
        try
        {
            if (!_interval.CanProceed()) 
            { 
                return Success.Result; 
            }

            switch (_engineState)
            {
                case EngineState.Running:
                    _engineState = EngineState.Stopping;
                    _bus.Publish(new StopRequestEvent { Exchange = BookExchange.Amazon });
                    break;
                case EngineState.NotRunning:
                    _engineState = EngineState.Running;
                    _thread = new Thread(() => _ = run(isbnList));
                    _thread.Start();
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
        return Success.Result;
    }

    private async Task run(List<string> isbnList)
    {
        await _amazonAccess.StartRefreshTokenInterval();

        var lookupResult = await getAmazonLookupData(isbnList);
        if (lookupResult.Error is not null)
        {
            publishErrorAndStop(lookupResult.Error);
            return;
        }
        var lookups = lookupResult.Value!;

        if (_needToStopThread)
        {
            stopThread();
            return;
        }
        
        var importResult = await importNewAmazonBookDataFrom(lookups);
        
        if (importResult.Error is not null)
        {
            publishErrorAndStop(importResult.Error);
            return;
        }
        
        if (_needToStopThread)
        {
            stopThread();
            return;
        }

        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _engineState = EngineState.NotRunning;

        publishNewStatus("Success!");
    }

    private void stopThread()
    {
        _bus.Publish(new ProgressEvent { Percent = 0, Exchange = BookExchange.Amazon });
        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _engineState = EngineState.NotRunning;
        publishNewStatus("Process was stopped");
    }

    private async Task<Result<Success, Exception>> importNewAmazonBookDataFrom(List<AmazonLookup> lookups)
    {
        publishNewStatus("Getting Amazon Book Prices 2/2");

        _db.ExecuteQuery("TRUNCATE TABLE [AmazonBookData]");

        var progress = new ProgressCounter(lookups.Count);
        foreach (var lookup in lookups)
        {
            if (_needToStopThread) { break; }
            var dataResult = await _amazonAccess.GetDataByLookup(lookup);
            if (dataResult.Error is not null)
            {
                // TODO: handle exception
            }
            var data = dataResult.Value!;
            data.ForEach(d => _db.InsertAmazonOutput(d));
            _bus.Publish(new ProgressEvent
            {
                Percent = progress.Increment(),
                Exchange = BookExchange.Amazon
            });
        }
        return Success.Result;
    }

    private async Task<Result<List<AmazonLookup>, Exception>> getAmazonLookupData(List<string> isbnList)
    {
        try
        {
            _bus.Publish(new StartEvent { Exchange = BookExchange.Amazon });
            publishNewStatus("Getting Amazon Lookups 1/2");

            var oldLookupResult = _db.GetAmazonLookup();
            if (oldLookupResult.Error != null)
            {
                return oldLookupResult.Error;
            }

            var oldLookups = oldLookupResult.Value!
                .Where(l => isbnList.Contains(l.ISBN13))
                .ToList();

            var noLookup = new List<string>();
            {
                var lookupDictionary = oldLookups
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

            var newLookups = new List<AmazonLookup>();
            var progress = new ProgressCounter(isbnList.Count);
            foreach (var lookup in noLookup)
            {
                if (_needToStopThread) { break; }
                var newLookupsResult = await _amazonAccess.GetLookupsFor(lookup);
                _bus.Publish(new ProgressEvent
                {
                    Percent = progress.Increment(),
                    Exchange = BookExchange.Amazon
                });

                if (newLookupsResult.Error is not null)
                {
                    return newLookupsResult.Error;
                }
                var lookups = newLookupsResult.Value!;
                lookups.ForEach(l => _db.InsertAmazonLookup(l));
                newLookups.AddRange(lookups);
            }

            return oldLookups.Plus(newLookups);
        }
        catch (Exception ex)
        {
            return ex;
        }
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