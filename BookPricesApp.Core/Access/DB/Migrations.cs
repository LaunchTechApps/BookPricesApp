namespace BookPricesApp.Core.Access.DB;

internal class Migrations
{
    private static string _combinedDataView = @"
        	IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'VW_CombinedBookData'))
                BEGIN
                    EXEC('
                        CREATE VIEW VW_CombinedBookData AS
                        SELECT 
                            ISBN, ItemId, Title, Seller, Location, ShippingPrice, 
                            Price, Condition, ItemUrl, Source, Error
                        FROM AmazonBookData
                        UNION ALL
                        SELECT 
                            ISBN, ItemId, Title, Seller, Location, ShippingPrice, 
                            Price, Condition, ItemUrl, Source, Error
                        FROM EBayBookData;
                    ');
                END
            ";
    private static string _createEbayTable = @"
            IF OBJECT_ID('EBayBookData', 'U') IS NULL
                BEGIN
                    CREATE TABLE EBayBookData (
                        ISBN VARCHAR(255) NOT NULL,
                        ItemId VARCHAR(255),
                        Title VARCHAR(255),
                        Seller VARCHAR(255),
                        Location VARCHAR(255),
                        ShippingPrice VARCHAR(MAX),
                        Price MONEY,
                        Condition VARCHAR(255),
                        ItemUrl VARCHAR(255),
                        Source VARCHAR(255),
                        Error VARCHAR(MAX)
                    );
                END";
    private static string _createAmazonTable = @"
            IF OBJECT_ID('AmazonBookData', 'U') IS NULL
                BEGIN
                    CREATE TABLE AmazonBookData (
                        ISBN VARCHAR(255) NOT NULL,
                        ItemId VARCHAR(255),
                        Title VARCHAR(255),
                        Seller VARCHAR(255),
                        Location VARCHAR(255),
                        ShippingPrice VARCHAR(MAX),
                        Price MONEY,
                        Condition VARCHAR(255),
                        ItemUrl VARCHAR(255),
                        Source VARCHAR(255),
                        Error VARCHAR(MAX)
                    );
                END";
    private string _createAmazonLookup = @"
            IF OBJECT_ID('AmazonLookup', 'U') IS NULL
               BEGIN
                   CREATE TABLE AmazonLookup (
                        ISBN13 VARCHAR(255) NOT NULL UNIQUE,
                        ASIN VARCHAR(255),
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
    private string _deleteOldLookups = @"
                DELETE FROM AmazonLookup
                WHERE LastUsed < DATEADD(day, -90, GETDATE());
            ";

    public string[] StartupScripts => new string[]
    {
        _createEbayTable,
        _createAmazonTable,
        _createAmazonLookup,
        _createIsbnFilePath,
        _deleteOldLookups,
        _combinedDataView
    };
}
