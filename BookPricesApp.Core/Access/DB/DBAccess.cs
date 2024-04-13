using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BookPricesApp.Core.Access.DB;
public class DBAccess
{
    private IConfiguration _config;
    private SqlConnection _connection 
    {
        get
        {
            var cs = _config.GetConnectionString("sqlExpress")!;
            var con = new SqlConnection(cs);
            return con;
        }
    }
    public DBAccess(IConfiguration config)
    {
        _config = config;
    }

    public Result<int, Exception> InsertAmazonLookup(AmazonLookup lookup)
    {
        try
        {
            string insertQuery = @"INSERT INTO AmazonLookup (ISBN13, ASIN, LastUsed, Title, URL, Error) 
                    VALUES (@ISBN13, @ASIN, @LastUsed, @Title, @URL, @Error)";

            using var con = _connection;
            con.Execute(insertQuery, lookup);
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
            using var con = _connection;
            var data = con.Query<AmazonLookup>(selectQuery).ToList();
            data = data ?? new List<AmazonLookup>();
            return data;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public Result<Success, Exception> ExecuteQuery(string query)
    {
        try
        {
            using var con = _connection;
            con.Execute(query);
        }
        catch (Exception ex)
        {
            return ex;
        }
        return Success.Result;
    }

    public Result<Success, Exception> SaveIsbnFilePath(BookExchange exchange, string path)
    {
        try
        {
            string insertQuery = @"INSERT INTO IsbnFilePaths (Exchange, FilePath) 
                    VALUES (@Exchange, @FilePath)";

            using var con = _connection;
            var item = new { Exchange = $"{exchange}", FilePath = path };
            con.Execute(insertQuery, item);
        }
        catch (Exception ex)
        {
            return ex;
        }
        return Success.Result;
    }

    public Result<string, Exception> GetIsbnFilePath(BookExchange exchange)
    {
        try
        {
            string query = @"SELECT FilePath FROM IsbnFilePaths
                            WHERE Exchange = @Exchange";
            var item = new { Exchange = $"{exchange}" };
            using var con = _connection;
            var path = con.Query<string>(query, item).FirstOrDefault();
            return path ?? "";
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public Result<Success, Exception> InsertAmazonOutput(ExportModel data)
    {
        try
        {
            string insertQuery = @"INSERT INTO AmazonBookData 
            (ISBN, ItemId, Title, Seller, Location, ShippingPrice, Price, Condition, ItemUrl, Source) 
                VALUES 
            (@ISBN, @ItemId, @Title, @Seller, @Location, @ShippingPrice, @Price, @Condition, @ItemUrl, @Source)";
            using var con = _connection;
            con.Execute(insertQuery, data);
        }
        catch (Exception ex)
        {
            return ex;
        }

        return Success.Result;
    }

    public Result<List<ExportModel>, Exception> GetExportFor(BookExchange exchange)
    {
        try
        {
            switch (exchange)
            {
                case BookExchange.Amazon:
                    {
                        string query = @"SELECT * FROM AmazonBookData";
                        var item = new { Exchange = $"{exchange}" };
                        using var con = _connection;
                        var data = con.Query<ExportModel>(query, item).ToList();
                        data = data ?? new List<ExportModel>();
                        return data;
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
    public Result<DBAccess, Exception> InitDB()
    {
        try
        {
            var tables = new Migrations();
            using var con = _connection;
            foreach (var query in tables.CreateTableArray)
            {
                con.Execute(query);
            }
        }
        catch (Exception ex)
        {
            return ex;
        }

        return this;
    }
}
