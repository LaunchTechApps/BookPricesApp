namespace BookPricesApp.Core.Domain.Config;
public class AppConfig
{
    public string Version { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public AmazonConfig Amazon { get; set; } = new();
    public Ebay Ebay { get; set; } = new();
}

public class AmazonConfig
{
    public string BaseUrl { get; set; } = string.Empty;
    public string IsbnFilePath { get; set; } = string.Empty;
    public AmazonAPIAccess ApiAccess { get; set; } = new();
}

public class Ebay
{
    public string BaseUrl { get; set; } = string.Empty;
    public string IsbnFilePath { get; set; } = string.Empty;
}

public class AmazonAPIAccess
{
    public string GrantType { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
