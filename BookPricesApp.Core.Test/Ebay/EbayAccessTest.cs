using BookPricesApp.Core.Access.Ebay;
using BookPricesApp.Core.Access.Ebay.Models;
using BookPricesApp.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace BookPricesApp.Core.Test.Ebay;
public class EbayAccessTest
{
    private IConfiguration _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

    private EventBus _bus = new EventBus();
    private ObjectFactory _objectFactory = new();
    public EbayAccessTest()
    {
        
    }

    [Fact]
    public void Should_Create_Keyword_Response_From_Json_Data()
    {
        var keyWordResponse = _objectFactory.CreateEbayKeyWordSearchResponse();
        var ebayModelFactory = new EbayModelFactory();

        var listings = ebayModelFactory.CreateExportFrom(new EbayKeywordResponseProps
        {
            ISBN = "1408855895",
            KeyWordResponse = keyWordResponse
        });

        Assert.True(listings.Count == 21);
    }

    [Fact]
    public async Task When_Accessing_Ebay_Data_No_Errors_Occur()
    {
        var ebayAccess = new EbayAccess(_config);
        var apiKey = _config.GetSection("ebay:apiKeys")
            .GetChildren()
            .Select(c => c.Value?.Trim() ?? "")
            .Where(key => !string.IsNullOrEmpty(key))
            .FirstOrDefault();

        var result = await ebayAccess.GetExportDataSingle("1408855895");
        
        Assert.False(result.DidError);
    }

    [Theory]
    [InlineData(1, 1, null)]
    [InlineData(35, 18, null)]
    [InlineData(1000, 250, null)]
    [InlineData(1, 2, "ran out of api keys")]
    [InlineData(10, 15, "ran out of api keys")]
    [InlineData(0, 1, "api key list is empty")]
    public void ApiKeyModel_Should_Handle_Being_Used(int amount, int useAmount, string? error)
    {
        List<string> apiKeys = _objectFactory.GenerateApiKeys(amount);
        var config = _objectFactory.CreateConfigurationWith("apiKeys", apiKeys);
        var apiKeysModel = new ApiKeysModel(config);
        
        TResult<string>? result = null;
        for (int i = 0; i < useAmount; i++)
        {
            result = apiKeysModel.GetNextApiKey();
        }

        if (!string.IsNullOrEmpty(error)) 
        {
            var actualError = result?.Error!.Description;
            var testResult = actualError == error;
            Assert.True(testResult);
            return;
        }

       Assert.False(result?.DidError);
    }
}