using Newtonsoft.Json;

namespace BookPricesApp.Core.Domain.Config;
public class AppConfig
{
    public static string FilePath
    {
        get
        {
            var subPath = "\\Domain\\Config\\";
            var mainPath = Directory.GetCurrentDirectory();
            return mainPath + subPath + "config.json";
        }
    }

    public string Version { get; set; } = string.Empty;
    public Amazon Amazon { get; set; } = new();

    public AppConfig()
    {
        var configFile = File.ReadAllText(FilePath);
        var config = JsonConvert.DeserializeObject<AppConfig>(configFile);
        Version = config?.Version ?? "";
        Amazon = config?.Amazon ?? new();
    }
}

public class Amazon
{
    public string BaseUrl { get; set; } = string.Empty;
    public string IsbnFilePath { get; set; } = string.Empty;
    public Apiaccess ApiAccess { get; set; } = new();
}

public class Apiaccess
{
    public string GrantType { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
