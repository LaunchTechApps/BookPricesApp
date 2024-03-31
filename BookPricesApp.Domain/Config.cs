namespace BookPricesApp.Domain;
public class Config
{
    public string Version { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string OutputPath => $"{DBFolderPath}\\Output.xlsx";
    public AmazonConfig Amazon { get; set; } = new();
    public Ebay Ebay { get; set; } = new();
    public static string DBFolderPath
    {
        get
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var appData = Environment.GetFolderPath(folder);
            return $"{appData}\\BookPriceFiles";
        }
    }
    public static string DBName = "BookPrices.db";
    public static string DBFilePath => $"{DBFolderPath}\\{DBName}";
}

public class AmazonConfig
{
    public string BaseUrl { get; set; } = string.Empty;
    public AmazonAPIAccess ApiAccess { get; set; } = new();
}

public class Ebay
{
    public string BaseUrl { get; set; } = string.Empty;
}

public class AmazonAPIAccess
{
    public string GrantType { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
