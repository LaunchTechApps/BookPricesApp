using BookPricesApp.Core.Access;
using BookPricesApp.Core.Access.DB;
using BookPricesApp.Core.Access.Ebay;
using BookPricesApp.Core.Access.Ebay.Models;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine.Contract;
using BookPricesApp.Core.Engine.Models;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using Microsoft.Extensions.Configuration;

namespace BookPricesApp.Core.Engine;
public class EbayEngine : IExchangeEngine
{
    private BookExchange _exchange = BookExchange.Ebay;
    private bool _needToStopThread => _engineState == EngineState.Stopping;
    private EventBus _bus;
    private EventInterval _interval = new EventInterval();
    private EngineState _engineState = EngineState.NotRunning;
    private EbayAccess _ebayAccess;
    private DBAccess _db;
    private ApiKeysModel? _apiKeyModel;
    private IConfiguration _config;

    private Thread? _thread;

    public EbayEngine(IConfiguration config,
        EventBus bus,
        EbayAccess ebayAccess,
        DBAccess db)
    {
        _config = config;
        _bus = bus;
        _ebayAccess = ebayAccess;
        _db = db;
    }
    public TResult<TVoid> Run(List<string> isbnList)
    {
        try
        {
            if (!_interval.CanProceed())
            {
                return TResult.Void;
            }

            switch (_engineState)
            {
                case EngineState.Running:
                    _engineState = EngineState.Stopping;
                    _bus.Publish(new StopRequestEvent { Exchange = BookExchange.Ebay });
                    break;
                case EngineState.NotRunning:
                    _apiKeyModel = new ApiKeysModel(_config);
                    var apiKeyResult = _apiKeyModel.GetNextApiKey();
                    if (apiKeyResult.DidError)
                    {
                        publishErrorAndStop(apiKeyResult.Error);
                        return apiKeyResult.Error;
                    }

                    _ebayAccess.SetApiKey(apiKeyResult.Value);
                    
                    _thread = new Thread(() => _ = run(isbnList));
                    _thread.Start();
                    
                    _engineState = EngineState.Running;
                    break;
                case EngineState.Stopping: break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
        return TResult.Void;
    }

    private async Task run(List<string> isbnList)
    {
        try
        {
            _bus.Publish(new StartEvent { Exchange = _exchange });
            publishNewStatus("Getting eBay Book Data");
            _db.ExecuteQuery("TRUNCATE TABLE [EBayBookData]");

            var progress = new ProgressCounter(isbnList.Count);

            for (var i = 0; i < isbnList.Count; i++)
            {
                var isbn = isbnList[i];
                if (_needToStopThread) { break; }
                var exportDataResult = await _ebayAccess.GetExportDataSingle(isbn);
                if (rateLimitExceeded(exportDataResult))
                {
                    var nextApiKey = _apiKeyModel!.GetNextApiKey();
                    if (nextApiKey.DidError)
                    {
                        publishErrorAndStop(nextApiKey.Error);
                        return;
                    }
                    _ebayAccess.SetApiKey(nextApiKey.Value);
                    i--;
                    continue;
                }
                
                if (exportDataResult.DidError)
                {
                    publishErrorAndStop(exportDataResult.Error);
                }
                
                exportDataResult.Value.ForEach(d => _db.InsertBookData(d));
                _bus.Publish(new ProgressEvent
                {
                    Percent = progress.Increment(),
                    Exchange = BookExchange.Ebay
                });
            }

            if (_needToStopThread)
            {
                stopThread();
                publishNewStatus("Successfully stopped");
                return;
            }

            publishNewStatus("Success!");
            _bus.Publish(new StopEvent { Exchange = _exchange });
            _engineState = EngineState.NotRunning;
        }
        catch (Exception ex)
        {
            publishErrorAndStop(Error.From(ex));
        }
    }

    private bool rateLimitExceeded(TResult<List<ExportModel>> exportDataResult)
    {
        if (!exportDataResult.DidError)
        {
            return false;
        }
        if (exportDataResult.Error == EBayAccessError.RateLimit)
        {
            return true;
        }
        
        return false;
    }

    private void stopThread()
    {
        _bus.Publish(new ProgressEvent { Percent = 0, Exchange = BookExchange.Ebay });
        _bus.Publish(new StopEvent { Exchange = BookExchange.Ebay });
        _engineState = EngineState.NotRunning;
        publishNewStatus("Process was stopped");
    }

    private void publishErrorAndStop(Error error)
    {
        publishNewStatus("...");
        _engineState = EngineState.NotRunning;
        _bus.Publish(new StopEvent { Exchange = BookExchange.Ebay });
        _bus.Publish(error);
    }

    private void publishNewStatus(string status)
    {
        _bus.Publish(new StatusLabelChangeEvent
        {
            Exchange = BookExchange.Ebay,
            Status = status
        });
    }
}
