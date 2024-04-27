using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using Microsoft.Extensions.Configuration;
using RestSharp;
using static Newtonsoft.Json.JsonConvert;
using BookPricesApp.Core.Access.Ebay.Models;

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
            
            var request = new RestRequest(resource, Method.Get);
            
            request.AddQueryParameter("OPERATION-NAME", "findItemsByProduct");
            request.AddQueryParameter("SERVICE-VERSION", "1.0.0");
            request.AddQueryParameter("SECURITY-APPNAME", _apiKey);
            request.AddQueryParameter("RESPONSE-DATA-FORMAT", "JSON");
            request.AddQueryParameter("REST-PAYLOAD", "");
            request.AddQueryParameter("paginationInput.entriesPerPage", "20");
            request.AddQueryParameter("productId.@type", "ISBN");
            request.AddQueryParameter("productId", isbn);
            request.AddQueryParameter("outputSelector", "SellerInfo");

            request.AddHeader("X-EBAY-SOA-SECURITY-APPNAME", _apiKey);
            request.AddHeader("X-EBAY-SOA-OPERATION-NAME", "findItemsByProduct");

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
                var content = DeserializeObject<EbayProductResponse>(response.Content);
                var exports = _modelFactory.CreateExportFrom(new EbayProductResponseProps
                {
                    ISBN = isbn,
                    KeyWordResponse = content
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
