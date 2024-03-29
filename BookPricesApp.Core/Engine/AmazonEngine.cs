using BookPricesApp.Core.Access.Amazon;
using BookPricesApp.Core.Access.FlatFile;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine.Models;
using BookPricesApp.Core.Utils;
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
    private EventBus _bus;
    private EventInterval _interval = new EventInterval();
    private IFlatFileAccess _flatFileAccess;
    private EngineState _engineState = EngineState.NotRunning;
    private AmazonAccess _amazonAccess;

    public AmazonEngine(EventBus bus, AmazonAccess amazonAccess, IFlatFileAccess flatFileAccess)
    {
        _bus = bus;
        _amazonAccess = amazonAccess;
        _flatFileAccess = flatFileAccess;
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
        //new Thread(progress).Start();
        //return;
        
        var lookupResult = _flatFileAccess.GetLookupListFor(BookExchange.Amazon);
        if (lookupResult.Ex != null)
        {
            // handle exception
        }
        if (lookupResult.Data == null)
        {
            lookupResult.Data = new List<BookLookup>();
        }

        var combinedList =  combine(lookupResult.Data, isbnList);
        var withLookup = combinedList.Where(b => b.HasLookup).ToList();
        var noLookup = combinedList.Where(b => !b.HasLookup).ToList();

        var hasLookupList = await _amazonAccess.GetDataByLookup(withLookup);
        var noLookupList = await _amazonAccess.GetDataByKeyWords(noLookup);

        var combined = new List<ExportModel>();
        combined.AddRange(hasLookupList.Data!);
        combined.AddRange(noLookupList.Data!);

        var booksDataTable = new DataTable();

        // get 
        // TODO: every minute, print out a new excel file
    }

    private DataTable combine(List<ExportModel> hasLookupList, List<ExportModel> noLookupList)
    {

        return new DataTable();
    }

    private List<BookLookup> combine(List<BookLookup> lookup, List<string> isbnList)
    {
        var preventDupes = new Dictionary<string, bool>();
        var result = new List<BookLookup>();
        foreach (var isbn in isbnList)
        {
            if (preventDupes.ContainsKey(isbn)) { continue; }
            preventDupes.Add(isbn, true);

            var isbnLookup = lookup.FirstOrDefault(l => l.ISBN13 == isbn);
            if (isbnLookup != null)
            {
                result.Add(isbnLookup);
            }
            else
            {
                result.Add(new BookLookup 
                { 
                    Exchange = $"{BookExchange.Amazon}", 
                    ISBN13 =  isbn 
                });
            }
        }
        return result;
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
                Count = count,
                Exchange = BookExchange.Amazon
            });
        }

        Thread.Sleep(2750);
        _bus.Publish(new StopEvent { Exchange = BookExchange.Amazon });
        _engineState = EngineState.NotRunning;
    }
}