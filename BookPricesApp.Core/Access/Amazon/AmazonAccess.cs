using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Config;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace BookPricesApp.Core.Access.Amazon;

public interface IAmazonAccess { }
public class AmazonAccess : IAmazonAccess
{
    private AmazonConfig _config;
    private string _email;
    private EventBus? _bus;
    private string? _accessToken;

    public AmazonAccess(AppConfig appConfig)
    {
        _config = appConfig.Amazon;
        _email = appConfig.Email;
    }

    public AmazonAccess(AppConfig appConfig, EventBus bus)
    {
        _config = appConfig.Amazon;
        _bus = bus;
        _email = appConfig.Email;
    }

    public async Task SetRefresToken()
    {
        var options = new RestClientOptions("https://api.amazon.com")
        {
            MaxTimeout = -1,
        };

        var client = new RestClient(options);
        var request = new RestRequest("/auth/O2/token", Method.Post);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddParameter("grant_type", _config.ApiAccess.GrantType);
        request.AddParameter("refresh_token", _config.ApiAccess.RefreshToken);
        request.AddParameter("client_id", _config.ApiAccess.ClientId);
        request.AddParameter("client_secret", _config.ApiAccess.ClientSecret);
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _bus?.Publish(new ErrorEvent
            {
                Title = "AmazonAccess.GetRefresToken",
                Message = response.ErrorMessage ?? "no error message given"
            });
        }
        else if (response.Content != null)
        {
            var responseObject = JObject.Parse(response.Content);
            _accessToken = responseObject["access_token"]?.ToString();
        }

        var test2 = "";
    }

    public async Task<Option<List<ExportModel>>> GetDataByLookup(List<BookLookup> withLookup)
    {
        throw new NotImplementedException();
    }

    public async Task<Option<List<ExportModel>>> GetDataByKeyWords(List<BookLookup> noLookup)
    {
        if (_accessToken == null)
        {
            var ex = new Exception("_accessToken was found null");
            return new Option<List<ExportModel>> { Ex = ex };
        }

        var path = "/products/2020-08-26/products";
        var query = "?locale=en_US&productRegion=US&facets=OFFERS&keywords=9780000248022";
        var client = new RestClient(new RestClientOptions("https://na.business-api.amazon.com")
        {
            MaxTimeout = -1,
        });
        var request = new RestRequest($"{path}{query}", Method.Get);
        request.AddHeader("x-amz-access-token", _accessToken);
        request.AddHeader("x-amz-user-email", _email);
        var response = await client.ExecuteAsync(request);

        return new Option<List<ExportModel>> { Data = new List<ExportModel>() { } };
    }
}
