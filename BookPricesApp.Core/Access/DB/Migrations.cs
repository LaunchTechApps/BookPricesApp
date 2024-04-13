namespace BookPricesApp.Core.Access.DB;

internal class Migrations
{
    public static string CreateOutputTable = @"
            IF OBJECT_ID('AmazonBookData', 'U') IS NULL
                BEGIN
                    CREATE TABLE AmazonBookData (
                        ISBN VARCHAR(255) NOT NULL,
                        ItemId VARCHAR(255) NOT NULL,
                        Title VARCHAR(255),
                        Seller VARCHAR(255),
                        Location VARCHAR(255),
                        ShippingPrice VARCHAR(MAX),
                        Price MONEY NOT NULL,
                        Condition VARCHAR(255),
                        ItemUrl VARCHAR(255),
                        Source VARCHAR(255)
                    );
                END";
    private string _createAmazonLookup = @"
            IF OBJECT_ID('AmazonLookup', 'U') IS NULL
               BEGIN
                   CREATE TABLE AmazonLookup (
                        ISBN13 VARCHAR(255) NOT NULL UNIQUE,
                        ASIN VARCHAR(255) NOT NULL UNIQUE,
                        Title VARCHAR(255),
                        URL VARCHAR(MAX),
                        LastUsed DATETIME,
                        Error VARCHAR(255)
                    );
                END";
    private string _createIsbnFilePath = @"
            IF OBJECT_ID('IsbnFilePaths', 'U') IS NULL
                BEGIN
                    CREATE TABLE IsbnFilePaths (
                        Exchange VARCHAR(255) NOT NULL UNIQUE,
                        FilePath VARCHAR(MAX) NOT NULL
                    );
                END";

    public string[] CreateTableArray => new string[]
    {
        CreateOutputTable,
        _createAmazonLookup,
        _createIsbnFilePath
    };
}
