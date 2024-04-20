using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BookPricesApp.Domain;

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

    public TResult<TVoid> UpsertAmazonLookup(AmazonLookup lookup)
    {
        try
        {
            var query = "SELECT * FROM AmazonLookup WHERE ISBN13 = @ISBN13";
            using var con = _connection;
            var oldLookup = con.Query<AmazonLookup>(query, lookup).FirstOrDefault();
            if (oldLookup is not null)
            {
                query = @"UPDATE AmazonLookup 
                            SET 
                                ASIN = @ASIN, LastUsed = @LastUsed, 
                                @Title = Title, URL = @URL, Error = @Error 
                            WHERE
                                ISBN13 = @ISBN13";

                con.Execute(query, lookup);
            }
            else
            {
                query = @"INSERT INTO AmazonLookup (ISBN13, ASIN, LastUsed, Title, URL, Error) 
                    VALUES (@ISBN13, @ASIN, @LastUsed, @Title, @URL, @Error)";

                con.Execute(query, lookup);
            }
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
        return TResult.Void;
    }

    public TResult<List<AmazonLookup>> GetAmazonLookup()
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
            return Error.From(ex);
        }
    }

    public TResult<TVoid> ExecuteQuery(string query)
    {
        try
        {
            using var con = _connection;
            con.Execute(query);
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
        return TResult.Void;
    }

    public TResult<TVoid> SaveIsbnFilePath(BookExchange exchange, string path)
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
            return Error.From(ex);
        }
        return TResult.Void;
    }

    public TResult<string> GetIsbnFilePath(BookExchange exchange)
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
            return Error.From(ex);
        }
    }

    public TResult<TVoid> InsertAmazonOutput(ExportModel data)
    {
        try
        {
            string insertQuery = @"INSERT INTO AmazonBookData 
            (ISBN, ItemId, Title, Seller, Location, ShippingPrice, Price, 
            Condition, ItemUrl, Source, Error) 
                VALUES 
            (@ISBN, @ItemId, @Title, @Seller, @Location, @ShippingPrice, 
            @Price, @Condition, @ItemUrl, @Source, @Error)";
            using var con = _connection;
            con.Execute(insertQuery, data);
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }

        return TResult.Void;
    }

    public TResult<List<ExportModel>> GetExportFor(BookExchange exchange)
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
            return Error.From(ex);
        }
    }
    public TResult<DBAccess> InitDB()
    {
        try
        {
            var tables = new Migrations();
            using var con = _connection;
            foreach (var query in tables.StartupScripts)
            {
                con.Execute(query);
            }
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }

        return this;
    }
}
