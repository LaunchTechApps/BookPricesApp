using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BookPricesApp.Repo;
public class BookPriceRepo
{
    public Option InsertAmazonLookup(List<AmazonLookup> list)
    {
		try
		{
            lock (Global.LockObject)
            {
                string insertQuery = @"INSERT INTO AmazonLookup (ISBN13, ASIN, LastUsed, Title, URL, Error) 
                       VALUES (@ISBN13, @ASIN, @LastUsed, @Title, @URL, @Error)";

                using var con = new SQLiteConnection(Global.CS);
                con.Open();
                foreach (var item in list)
                {
                    con.Execute(insertQuery, item);
                }
                con.Close();
            }
        }
		catch (Exception ex)
		{
			return new(ex);
		}
        return new();
    }

    public Option<List<AmazonLookup>> GetAmazonLookup()
    {
        try
        {
            lock (Global.LockObject)
            {
                string selectQuery = @"SELECT * FROM AmazonLookup";
                using var con = new SQLiteConnection(Global.CS);
                con.Open();
                var lookupData = con.Query<AmazonLookup>(selectQuery).ToList();
                con.Close();
                return new Option<List<AmazonLookup>> { Data = lookupData };
            }
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public Option ExecuteQuery(string query)
    {
        try
        {
            lock (Global.LockObject)
            {
                using var con = new SQLiteConnection(Global.CS);
                con.Open();
                con.Execute(query);
                con.Close();
            }
        }
        catch (Exception ex)
        {
            return new(ex);
        }
        return new();
    }

    public Option SaveIsbnFilePath(BookExchange exchange, string path)
    {
        try
        {
            lock (Global.LockObject)
            {
                string insertQuery = @"INSERT INTO IsbnFilePaths (Exchange, FilePath) 
                       VALUES (@Exchange, @FilePath)";

                using var con = new SQLiteConnection(Global.CS);
                con.Open();
                var item = new { Exchange = $"{exchange}", FilePath = path };
                con.Execute(insertQuery, item);
                con.Close();
            }
        }
        catch (Exception ex)
        {
            return new(ex);
        }
        return new();
    }

    public Option<string> GetIsbnFilePath(BookExchange exchange)
    {
        try
        {
            lock (Global.LockObject)
            {
                string query = @"SELECT FilePath FROM IsbnFilePaths
                                WHERE Exchange = @Exchange";
                using var con = new SQLiteConnection(Global.CS);
                con.Open();
                var item = new { Exchange = $"{exchange}" };
                con.Execute(query, item);
                var path = con.Query<string>(query, item).FirstOrDefault();
                con.Close();
                return new Option<string> { Data = path ?? "" };
            }
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public Option InsertAmazonOutput(List<ExportModel> list)
    {
        try
        {
            lock (Global.LockObject)
            {
                string insertQuery = @"INSERT INTO Output 
                (ISBN, ItemId, Title, Seller, Location, ShippingPrice, Price, Condition, ItemUrl, Source) 
                    VALUES 
                (@ISBN, @ItemId, @Title, @Seller, @Location, @ShippingPrice, @Price, @Condition, @ItemUrl, @Source)";

                using var con = new SQLiteConnection(Global.CS);
                con.Open();
                foreach (var item in list)
                {
                    con.Execute(insertQuery, item);
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            return new(ex);
        }
        return new();
    }
}
