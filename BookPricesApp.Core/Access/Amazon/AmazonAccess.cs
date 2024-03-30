using BookPricesApp.Core.Access.Amazon.Models;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Config;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Utils;
using Newtonsoft.Json;
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
    private EventBus _bus;
    private string? _accessToken;

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
    }

    public async Task<Option<List<ExportModel>>> GetDataByLookup(List<AmazonLookup> withLookup)
    {
        throw new NotImplementedException();
    }

    public async Task<Option<List<AmazonLookup>>> GetLookupsFor(List<string> isbnList)
    {
        if (_accessToken == null)
        {
            var ex = new Exception("_accessToken was found null");
            return new Option<List<AmazonLookup>> { Ex = ex };
        }
        var result = new List<AmazonLookup>();
        var progress = new ProgressCounter(isbnList.Count);
        foreach (var isbn in isbnList)
        {
            var lookup = await getLookupFor(isbn);
            //Thread.Sleep(500);

            if (lookup.Ex != null)
            {
                result.Add(new AmazonLookup
                {
                    ISBN13 = isbn,
                    LastUsed = DateTime.UtcNow.ToIso8601(),
                    Error = lookup.Ex.Message
                });
            }
            else
            {
                lookup.Data?.ForEach(result.Add);
            }
            
            _bus.Publish(new ProgressEvent
            {
                Percent = progress.Increment(),
                Exchange = BookExchange.Amazon
            });
        }

        return new Option<List<AmazonLookup>> { Data = result };
    }

    private async Task<Option<List<AmazonLookup>>> getLookupFor(string isbn)
    {
        var result = new List<AmazonLookup>();
        try
        {
            var path = "/products/2020-08-26/products";
            var query = $"?locale=en_US&productRegion=US&facets=OFFERS&keywords={isbn}";
            var client = new RestClient(new RestClientOptions(_config.BaseUrl)
            {
                MaxTimeout = -1,
            });
            var request = new RestRequest($"{path}{query}", Method.Get);
            request.AddHeader("x-amz-access-token", _accessToken!);
            request.AddHeader("x-amz-user-email", _email);
            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new Option<List<AmazonLookup>> { Ex = new Exception(response.ErrorMessage) };
            }
            else if (response.Content == null)
            {
                var message = "response.Content in AmazonAccess.GetDataByKeyWords was found null";
                return new Option<List<AmazonLookup>> { Ex = new Exception(message) };
            }
            else
            {
                var content = JsonConvert.DeserializeObject<ProductListing>(response.Content);
                foreach (var item in content?.Products ?? Array.Empty<Product>())
                {
                    result.Add(new AmazonLookup
                    {
                        ISBN13 = isbn,
                        ASIN = item.Asin,
                        LastUsed = DateTime.Now.ToIso8601(),
                    });
                }
            }
        }
        catch (Exception ex)
        {
            return new Option<List<AmazonLookup>> { Ex = ex };
        }

        return new Option<List<AmazonLookup>> { Data = result };
    }
}
