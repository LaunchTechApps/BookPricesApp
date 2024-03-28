using Newtonsoft.Json.Linq;

namespace BookPricesApp.Domain.Config;

internal class AppConfig
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
    public AmazonConfig AmazonConfig = new();
    public string Version = string.Empty;
    public AppConfig()
    {
        setup();
    }

    private void setup()
    {
        var configFile = File.ReadAllText(FilePath);
        var configObj = JObject.Parse(configFile);
        AmazonConfig.BaseUrl = configObj["amazon"]!["baseUrl"]!.ToString();
        Version = configObj["version"]!.ToString();
    }
}
