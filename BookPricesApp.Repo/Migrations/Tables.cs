namespace BookPricesApp.Repo.Migrations;
public class Tables
{
    public static string CreateOutputTable = @"
                CREATE TABLE IF NOT EXISTS [Output] (
	                [ISBN]		TEXT NOT NULL,
	                [ItemId]	TEXT NOT NULL,
	                [Title]		TEXT,
	                [Seller]	TEXT,
	                [Location]	TEXT,
	                [ShippingPrice]	TEXT,
	                [Price]		TEXT NOT NULL,
	                [Condition]	TEXT,
	                [ItemUrl]	TEXT,
	                [Source]	TEXT NOT NULL
                );";
    private string _createAmazonLookup = @"
                CREATE TABLE IF NOT EXISTS [AmazonLookup] (
	                [ISBN13]	TEXT NOT NULL UNIQUE,
	                [ASIN]		TEXT NOT NULL UNIQUE,
					[Title]		TEXT NOT NULL,
					[URL]		TEXT NOT NULL,
	                [LastUsed]	TEXT NOT NULL,
	                [Error]		TEXT
                );";
    private string _createIsbnFilePath = @"
                CREATE TABLE IF NOT EXISTS [IsbnFilePaths] (
	                [Exchange]		TEXT NOT NULL UNIQUE,
	                [FilePath]		TEXT NOT NULL
                );";

    public string[] CreateTableArray => new string[]
	{
		CreateOutputTable,
		_createAmazonLookup,
		_createIsbnFilePath
	};
}