using BookPricesApp.Core.Access.Amazon.Models;
using BookPricesApp.Core.Access.Contract;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using static Newtonsoft.Json.JsonConvert;
using RestSharp;
using BookPricesApp.Domain;
using BookPricesApp.Domain.Export;

namespace BookPricesApp.Core.Access.Amazon;

public interface IAmazonAccess { }
public class AmazonAccess : IAmazonAccess
{
    private IConfiguration _config;
    private EventBus _bus;
    private string? _accessToken;
    private AmazonModelFactory _exportFactory = new();

    public AmazonAccess(IConfiguration config, EventBus bus)
    {
        _config = config;
        _bus = bus;
    }

    private bool _runningRefreshToken = false;
    public async Task<TResult<TVoid>> StartRefreshTokenInterval()
    {
        try
        {
            void refreshTheRefreshToken()
            {
                if (_runningRefreshToken)
                {
                    return;
                }
                _runningRefreshToken = true;
                while (_runningRefreshToken)
                {
                    Thread.Sleep(Duration.Minutes(58));
                    _ = setRefresToken();
                }
            }

            new Thread(refreshTheRefreshToken).Start();
            await setRefresToken();
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
        return TResult.Void;
    }

    private async Task<TResult<TVoid>> setRefresToken()
    {
        try
        {
            var options = new RestClientOptions("https://api.amazon.com")
            {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest("/auth/O2/token", Method.Post);
            var test = _config.GetSection("amazon:apiAccess")["grantType"];
            request.AddParameter("grant_type", _config.GetSection("amazon:apiAccess")["grantType"]);
            request.AddParameter("refresh_token", _config.GetSection("amazon:apiAccess")["refreshToken"]);
            request.AddParameter("client_id", _config.GetSection("amazon:apiAccess")["clientId"]);
            request.AddParameter("client_secret", _config.GetSection("amazon:apiAccess")["clientSecret"]);
            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _bus?.Publish(new ErrorEvent
                {
                    Message = response.ErrorMessage ?? "no error message given"
                });
            }
            else if (response.Content != null)
            {
                var responseObject = JObject.Parse(response.Content);
                _accessToken = responseObject["access_token"]?.ToString();
            }
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
        return TResult.Void;
    }

    public async Task<TResult<List<ExportModel>>> GetDataByLookup(AmazonLookup lookup)
    {
        var result = new List<ExportModel>();
        try
        {
            var path = "/products/2020-08-26/products";
            var query = $"/{lookup.ASIN}/offers?locale=en_US&productRegion=US";
            var baseUrl = _config.GetSection("amazon")["baseUrl"]!;
            var client = new RestClient(new RestClientOptions(baseUrl)
            {
                MaxTimeout = -1,
            });
            var request = new RestRequest($"{path}{query}", Method.Get);
            request.AddHeader("x-amz-access-token", _accessToken!);
            request.AddHeader("x-amz-user-email", _config.GetSection("email").Value!);
            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return AmazonAccessError.BadRequest("AmazonAccess.GetDataByLookup", response);
            }
            else if (response.Content == null)
            {
                return AmazonAccessError.NullContect("AmazonAccess.GetDataByLookup");
            }
            else
            {
                var content = DeserializeObject<ProductOffers>(response.Content);
                var modelResult = _exportFactory.CreateExport(new AmazonExportProps
                {
                    Lookup = lookup,
                    Offers = content
                });
               result.AddRange(modelResult.Value);
            }
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }

        return result;
    }

    public async Task<TResult<List<AmazonLookup>>> GetLookupsFor(string isbn)
    {
        if (_accessToken == null)
        {
            return AmazonAccessError.NullAccessToken();
        }

        var result = new List<AmazonLookup>();
        var lookupResult = await getLookupFor(isbn);

        if (lookupResult.DidError)
        {
            result.Add(new AmazonLookup
            {
                ISBN13 = isbn,
                LastUsed = DateTime.Now,
                Error = lookupResult.Error.Message
            });
        }
        else
        {
            lookupResult.Value?.ForEach(result.Add);
        }

        return result;
    }

    private async Task<TResult<List<AmazonLookup>>> getLookupFor(string isbn)
    {
        var result = new List<AmazonLookup>();
        try
        {
            var path = "/products/2020-08-26/products";
            var query = $"?locale=en_US&productRegion=US&facets=OFFERS&keywords={isbn}";
            var baseUrl = _config.GetSection("amazon")["baseUrl"]!;
            var client = new RestClient(new RestClientOptions(baseUrl)
            {
                MaxTimeout = -1,
            });
            var request = new RestRequest($"{path}{query}", Method.Get);
            request.AddHeader("x-amz-access-token", _accessToken!);
            request.AddHeader("x-amz-user-email", _config.GetSection("email").Value!);
            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return AmazonAccessError.BadRequest("AmazonAccess.getLookupFor", response);
            }
            else if (response.Content == null)
            {
                return AmazonAccessError.NullContect("AmazonAccess.getLookupFor");
            }
            else
            {
                var content = DeserializeObject<ProductListing>(response.Content);
                var modelsResult = _exportFactory.CreateLookup(new AmazonLookupProps
                {
                    Listing = content,
                    ISBN = isbn,
                });
                result.AddRange(modelsResult.Value);
            }
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }

        return result;
    }
}