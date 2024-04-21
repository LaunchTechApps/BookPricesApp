using BookPricesApp.Core.Access.Ebay.Contract;
using BookPricesApp.Domain.Files;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using static Newtonsoft.Json.JsonConvert;

namespace BookPricesApp.Core.Test;
internal class ObjectFactory
{
    public static List<string> ApiKeys = ["1", "2"];
    private string _testDataPath => Directory.GetCurrentDirectory() + "\\Data";
    internal AmazonLookup CreateAmazonLookup()
    {
        var path = $"{_testDataPath}\\test.amazon-keyword-search.json";
        var json = File.ReadAllText(path);
        return DeserializeObject<AmazonLookup>(json)!;
    }

    internal ProductOffers CreateAmazonProductOffers()
    {
        var path = $"{_testDataPath}\\test.amazon-offer.json";
        var json = File.ReadAllText(path);
        return DeserializeObject<ProductOffers>(json)!;
    }

    internal EbayKeyWordResponse CreateEbayKeyWordSearchResponse()
    {
        var path = $"{_testDataPath}\\test.ebay-keyword-response.json";
        var json = File.ReadAllText(path);
        return DeserializeObject<EbayKeyWordResponse>(json)!;
    }

    internal List<string> GenerateApiKeys(int amount)
    {
        return Enumerable.Range(0, amount)
            .Select(x => $"{x + 1}")
            .ToList();
    }

    internal IConfiguration CreateConfigurationWith(string key, List<string> apiKeys)
    {
        var json = new JObject(
                new JProperty("ebay", new JObject(
                        new JProperty("baseUrl", "https://svcs.ebay.com"),
                        new JProperty(key, new JArray(apiKeys))
                    )
                )
            ).ToString();

        var jsonEncoded = Encoding.UTF8.GetBytes(json);
        var jsonStream = new MemoryStream(jsonEncoded);

        var config = new ConfigurationBuilder()
            .AddJsonStream(jsonStream)
            .Build();

        return config;
    }
}
