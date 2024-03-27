using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Utils.Config;

internal class AppConfig
{
    public AmazonConfig AmazonConfig = new();
    public AppConfig()
    {
        setup();
    }

    private void setup()
    {
        var path = Directory.GetCurrentDirectory() + "\\Config\\";
        var configFile = File.ReadAllText(path + "config.json");
        var configObj = JObject.Parse(configFile);
        AmazonConfig.BaseUrl = configObj["amazon"]!["baseUrl"]!.ToString();
        var test = "";
    }
}
