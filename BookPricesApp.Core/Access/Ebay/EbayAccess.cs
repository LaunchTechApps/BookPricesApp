using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using Microsoft.Extensions.Configuration;
using RestSharp;
using static Newtonsoft.Json.JsonConvert;
using BookPricesApp.Core.Access.Ebay.Models;
using Newtonsoft.Json.Linq;

namespace BookPricesApp.Core.Access.Ebay;
public class EbayAccess
{
    private IConfiguration _config;
    private ApiKeysModel _apiKeyModel;
    private string? _apiKey;
    private EbayModelFactory _modelFactory;

    public EbayAccess(IConfiguration config)
    {
        _config = config;

        _apiKeyModel = new ApiKeysModel(_config);
        var apiKeyResult = _apiKeyModel.GetNextApiKey();

        if (!apiKeyResult.DidError)
        {
            _apiKey = apiKeyResult.Value;
        }

        _modelFactory = new EbayModelFactory();
    }

    public async Task<TResult<List<ExportModel>>> GetExportDataSingle(string isbn)
    {
        try
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                return Error.Create("EbayAccess.GetExportDataSingle", "api key not set");
            }

            var result = new List<ExportModel>();
         
            var baseUrl = _config.GetSection("ebay:baseUrl").Value!;

            var resource = "/services/search/FindingService/v1";

            var options = new RestClientOptions(baseUrl);
            var client = new RestClient(new RestClientOptions(baseUrl));
            
            var request = new RestRequest(resource, Method.Post);

            request.AddQueryParameter("Response-Data-Format", "json");
            request.AddQueryParameter("Request-Data-Format", "json");

            request.AddHeader("X-EBAY-SOA-SECURITY-APPNAME", _apiKey);
            request.AddHeader("X-EBAY-SOA-OPERATION-NAME", "findItemsByKeywords");
            request.AddHeader("Content-Type", "application/json");

            var jOject = new JObject(
                    new JProperty("@xmlns", "http://www.ebay.com/marketplace/search/v1/services"),
                    new JProperty("keywords", isbn),
                    new JProperty("itemFilter", 
                        new JArray 
                        { 
                            new JObject(
                                    new JProperty("name", "MinQuantity"),
                                    new JProperty("value", "1")
                                ),
                            new JObject(
                                    new JProperty("name", "HideDuplicateItems"),
                                    new JProperty("value", "true")
                                )
                        }),
                    new JProperty("outputSelector", "SellerInfo"),
                    new JProperty("paginationInput", new JObject(
                            new JProperty("entriesPerPage", 110),
                            new JProperty("pageNumber", 1)
                        ))
                ).ToString(Newtonsoft.Json.Formatting.None);

            request.AddStringBody(jOject, DataFormat.Json);

            var response = await client.ExecuteAsync(request);
            if (rateLimitExceeded(response))
            {
                var apiKeyResult = _apiKeyModel.GetNextApiKey();
                if (apiKeyResult.DidError)
                {
                    return EBayAccessError.RateLimit;
                }
                _apiKey = apiKeyResult.Value;
                // a little recursion. couldn't think of a better way cept maybe a GOTO statement?
                return await GetExportDataSingle(isbn);
            }
            else if (!response.IsSuccessStatusCode)
            {
                var code = $"code: {response.StatusCode}";
                var errorMessage = $"{code}\n{response.ErrorMessage}\n{response.Content}";
                return Error.Create("EbayAccess.GetExportDataSingle", errorMessage);
            }
            else if (response.Content == null)
            {
                return Error.Create("EbayAccess.GetExportDataSingle", "content was found null");
            }
            else
            {
                var content = DeserializeObject<EbayKeyWordResponse>(response.Content);
                var exports = _modelFactory.CreateExportFrom(new EbayKeywordResponseProps
                {
                    KeyWordResponse = content,
                    ISBN = isbn
                });
                result.AddRange(exports);
            }
            return result;
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
    }

    private bool rateLimitExceeded(RestResponse response)
    {
        if (response.IsSuccessful)
        {
            return false;
        }

        if (string.IsNullOrEmpty(response.Content))
        { 
            return false; 
        }
        
        var rateExceeded = "call has exceeded the number";
        if (response.Content.Contains(rateExceeded))
        {
            return true;
        }

        return false;
    }
}
