﻿using BookPricesApp.Core.Access.DB;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Engine;
using BookPricesApp.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace BookPricesApp.Core.Manager.App;

public interface IAppManager
{
    void SubmitMainEvent(BookExchange exchange, string isbnFilePath);
}

public class AppManager : IAppManager
{
    private EngineProvider _engineProvider { get; set; }
    private EventBus _bus;
    private IConfiguration _config;
    private DBAccess _db;
    public AppManager(
        EngineProvider engineProvider,
        DBAccess db,
        EventBus bus,
        IConfiguration config)
    {
        _engineProvider = engineProvider;
        _bus = bus;
        _db = db;
        _config = config;
    }
    public void SubmitMainEvent(BookExchange exchange, string isbnFilePath)
    {
        if (!File.Exists(isbnFilePath))
        {
            _bus.Publish(new AlertEvent($"File not found: {isbnFilePath}"));
            return;
        }

        var isbnList = File.ReadAllLines(isbnFilePath)
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => s.Trim())
            .ToList();

        var firstISBN = isbnList.FirstOrDefault() ?? "";
        if (!ISBNValidation.IsValidISBN(firstISBN))
        {
            _bus.Publish(new AlertEvent($"Fisrt line was an invalid ISBN: {firstISBN}"));
            return;
        }

        var savedIsbnFileResult = _db.UpsertIsbnFilePath(exchange, isbnFilePath);
        if (savedIsbnFileResult.DidError)
        {
            _bus.Publish(savedIsbnFileResult.Error);
        }

        var exchangeEngineResult = _engineProvider.GetExchange(exchange);
        if (exchangeEngineResult.DidError)
        {
            _bus.Publish(exchangeEngineResult.Error);
            return;
        }
        exchangeEngineResult.Value.Run(isbnList);
    }
}
