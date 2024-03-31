using BookPricesApp.Core.Access.FlatFile;
using BookPricesApp.Domain;
using BookPricesApp.Domain.Files;
using System.Data;

namespace BookPricesApp.Tests;
public class BasicTests
{
    [Fact]
    public void Test1()
    {
        var db = new FlatFileAccess();

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
    }
}
