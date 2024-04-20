using BookPricesApp.Domain.Files;
using static Newtonsoft.Json.JsonConvert;

namespace BookPricesApp.Core.Test;
internal class ObjectFactory
{
    private string _testDataPath => Directory.GetCurrentDirectory() + "\\Data";
    internal AmazonLookup CreateAmazonLookup()
    {
        var path = $"{_testDataPath}\\test.keyword-search.json";
        var json = File.ReadAllText(path);
        return DeserializeObject<AmazonLookup>(json)!;
    }

    internal ProductOffers CreateAmazonProductOffers()
    {
        var path = $"{_testDataPath}\\test.offer.json";
        var json = File.ReadAllText(path);
        return DeserializeObject<ProductOffers>(json)!;
    }
}
