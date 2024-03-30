using BookPricesApp.Core.Access.Amazon;
using BookPricesApp.Core.Access.FlatFile;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Export;
using BookPricesApp.Core.Utils;
using System.Data;

namespace BookPricesApp.Tests;
public class BasicTests
{
    [Fact]
    public void Test1()
    {
        var db = new FlatFileAccess(new EventBus());
        var config = db.GetAppConfig().Data!;

        var firstVersion = "0.0.0";
        config.Version = firstVersion;
        db.SaveAppConfig();
        config = db.GetAppConfig().Data!;
        Assert.True(config.Version == firstVersion);

        var secondVersion = "0.0.1";
        config.Version = secondVersion;
        db.SaveAppConfig();
        config = db.GetAppConfig().Data!;
        Assert.True(config.Version == secondVersion);
    }

    [Fact]
    public void Test2()
    {
        var db = new FlatFileAccess(new EventBus());
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        path += "\\example.xlsx";
        db.CreateNewOutput(path);

        DataTable dt = new DataTable();

        dt.Columns.Add(COL.ISBN, typeof(string));
        dt.Columns.Add(COL.ItemId, typeof(long));
        dt.Columns.Add(COL.Title, typeof(string));
        dt.Columns.Add(COL.Seller, typeof(string));
        dt.Columns.Add(COL.Location, typeof(string));
        dt.Columns.Add(COL.ShippingPrice, typeof(double));
        dt.Columns.Add(COL.Price, typeof(double));
        dt.Columns.Add(COL.Condition, typeof(string));
        dt.Columns.Add(COL.ItemURL, typeof(string));
        dt.Columns.Add(COL.Source, typeof(string));

        var rnd = new Random();
        for (int i = 0; i < 100000; i++)
        {
            DataRow row = dt.NewRow();
            row[COL.ISBN] = $"978025333056{i}";
            row[COL.ItemId] = 3351 + i;
            row[COL.Title] = $"Book Title {i}";
            row[COL.Seller] = $"seller{i}";
            row[COL.Location] = $"Location{i}";
            row[COL.ShippingPrice] = 0 + i * 1.99;
            row[COL.Price] = 2.99 + (rnd.NextDouble() * (599 - 2.99));
            row[COL.Condition] = "Brand New";
            row[COL.ItemURL] = $"https://example.com/book/{i}";
            row[COL.Source] = "Ebay";

            dt.Rows.Add(row);
        }

        db.OutputAppend(dt);
    }

    [Fact]
    public async Task Test3()
    {
        var db = new FlatFileAccess(new EventBus());
        var config = db.GetAppConfig().Data!;
        var amazonAccess = new AmazonAccess(config, new EventBus());
        await amazonAccess.SetRefresToken();
        var test2 = "";
    }

    [Fact]
    public void Test4()
    {
        var db = new FlatFileAccess(new EventBus());
        var test1 = db.GetLookupListFor(Core.Domain.Types.BookExchange.Amazon);
        var test2 = "";
    }

    [Fact]
    public async Task Test5()
    {
        var db = new FlatFileAccess(new EventBus());
        var config = db.GetAppConfig().Data!;
        var amazonAccess = new AmazonAccess(config, new EventBus());
        await amazonAccess.SetRefresToken();
        var test1 = await amazonAccess.GetLookupsFor(new List<string>());
        var test2 = "";
    }
}
