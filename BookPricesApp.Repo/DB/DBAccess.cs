﻿using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using Dapper;
using System.Data.SqlClient;

namespace BookPricesApp.Access.DB;
public class DBAccess
{
    public Result<int, Exception> InsertAmazonLookup(List<AmazonLookup> list)
    {
        try
        {
            string insertQuery = @"INSERT INTO AmazonLookup (ISBN13, ASIN, LastUsed, Title, URL, Error) 
                    VALUES (@ISBN13, @ASIN, @LastUsed, @Title, @URL, @Error)";

            using var con = new SqlConnection(Global.CS);
            con.Open();
            foreach (var item in list)
            {
                con.Execute(insertQuery, item);
            }
            con.Close();
        }
        catch (Exception ex)
        {
            return ex;
        }
        return 0;
    }

    public Result<List<AmazonLookup>, Exception> GetAmazonLookup()
    {
        try
        {
            string selectQuery = @"SELECT * FROM AmazonLookup";
            using var con = new SqlConnection(Global.CS);
            con.Open();
            var lookupData = con.Query<AmazonLookup>(selectQuery).ToList();
            con.Close();
            return lookupData;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public Result<int, Exception> ExecuteQuery(string query)
    {
        try
        {
            using var con = new SqlConnection(Global.CS);
            con.Open();
            con.Execute(query);
            con.Close();
        }
        catch (Exception ex)
        {
            return ex;
        }
        return 0;
    }

    public Result<int, Exception> SaveIsbnFilePath(BookExchange exchange, string path)
    {
        try
        {
            string insertQuery = @"INSERT INTO IsbnFilePaths (Exchange, FilePath) 
                    VALUES (@Exchange, @FilePath)";

            using var con = new SqlConnection(Global.CS);
            con.Open();
            var item = new { Exchange = $"{exchange}", FilePath = path };
            con.Execute(insertQuery, item);
            con.Close();
        }
        catch (Exception ex)
        {
            return ex;
        }
        return 0;
    }

    public Result<string, Exception> GetIsbnFilePath(BookExchange exchange)
    {
        try
        {
            string query = @"SELECT FilePath FROM IsbnFilePaths
                            WHERE Exchange = @Exchange";
            using var con = new SqlConnection(Global.CS);
            con.Open();
            var item = new { Exchange = $"{exchange}" };
            con.Execute(query, item);
            var path = con.Query<string>(query, item).FirstOrDefault();
            con.Close();
            return path ?? "";
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public Result<int, Exception> InsertAmazonOutput(List<ExportModel> list)
    {
        try
        {
            string insertQuery = @"INSERT INTO Output 
            (ISBN, ItemId, Title, Seller, Location, ShippingPrice, Price, Condition, ItemUrl, Source) 
                VALUES 
            (@ISBN, @ItemId, @Title, @Seller, @Location, @ShippingPrice, @Price, @Condition, @ItemUrl, @Source)";

            using var con = new SqlConnection(Global.CS);
            con.Open();
            foreach (var item in list)
            {
                con.Execute(insertQuery, item);
            }
            con.Close();
        }
        catch (Exception ex)
        {
            return ex;
        }

        return 0;
    }

    public Result<List<ExportModel>, Exception> GetExportFor(BookExchange exchange)
    {
        try
        {
            switch (exchange)
            {
                case BookExchange.Amazon:
                    {
                        string query = @"SELECT * FROM Output";
                        using var con = new SqlConnection(Global.CS);
                        con.Open();
                        var item = new { Exchange = $"{exchange}" };
                        con.Execute(query, item);
                        var path = con.Query<ExportModel>(query, item).ToList();
                        con.Close();
                        return path;
                    }
                case BookExchange.Ebay:
                    throw new NotImplementedException("Ebay export not implemented");
                default:
                    throw new Exception("Uknown exchange");
            }
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
