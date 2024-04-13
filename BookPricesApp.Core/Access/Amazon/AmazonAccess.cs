using BookPricesApp.Core.Access.Amazon.Models;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BookPricesApp.Core.Access.Amazon;

public interface IAmazonAccess { }
public class AmazonAccess : IAmazonAccess
{
    private IConfiguration _config;
    private EventBus _bus;
    private string? _accessToken;

    public AmazonAccess(IConfiguration config, EventBus bus)
    {
        _config = config;
        _bus = bus;
    }

    private bool _runningRefreshToken = false;
    public async Task<Result<Success, Exception>> StartRefreshTokenInterval()
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
            return ex;
        }
        return Success.Result;
    }

    private async Task<Result<int, Exception>> setRefresToken()
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
            return ex;
        }
        return 0;
    }

    public async Task<Result<List<ExportModel>, Exception>> GetDataByLookup(AmazonLookup lookup)
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
                return new Exception(response.ErrorMessage);
            }
            else if (response.Content == null)
            {
                var message = "response.Content in AmazonAccess.GetDataByKeyWords was found null";
                return new Exception(message);
            }
            else
            {
                var content = JsonConvert.DeserializeObject<ProductOffers>(response.Content);
                var fo = content?.FeaturedOffer;
                if (fo != null)
                {
                    result.Add(new ExportModel
                    {
                        ISBN = lookup.ISBN13,
                        ItemId = lookup.ASIN ?? "",
                        Title = lookup.Title ?? "",
                        ItemUrl = lookup.URL ?? "",
                        Condition = $"{fo.Condition.ConditionValue}-{fo.Condition.SubCondition}",
                        Seller = fo.Merchant.Name ?? "",
                        Location = "US",
                        ShippingPrice = fo.DeliveryInformation ?? "",
                        Price = fo.Price.Value.Amount.ToString(),
                    });
                }

                foreach (var item in content?.Offers ?? Array.Empty<Offer>())
                {
                    result.Add(new ExportModel
                    {
                        ISBN = lookup.ISBN13,
                        ItemId = lookup.ASIN ?? "",
                        Title = lookup.Title ?? "",
                        ItemUrl = lookup.URL ?? "",
                        Condition = $"{item.Condition.ConditionValue}-{item.Condition.SubCondition}",
                        Seller = item.Merchant.Name ?? "",
                        Location = "US",
                        ShippingPrice = item.DeliveryInformation ?? "",
                        Price = item.Price.Value.Amount.ToString(),
                    });
                }
            }
        }
        catch (Exception ex)
        {
            return ex;
        }

        return result;
    }

    public async Task<Result<List<AmazonLookup>, Exception>> GetLookupsFor(string isbn)
    {
        if (_accessToken == null)
        {
            var ex = new Exception("_accessToken was found null");
            return ex;
        }

        var result = new List<AmazonLookup>();
        var lookup = await getLookupFor(isbn);

        if (lookup.Error is not null)
        {
            result.Add(new AmazonLookup
            {
                ISBN13 = isbn,
                LastUsed = DateTime.Now,
                Error = lookup.Error.Message
            });
        }
        else
        {
            lookup.Value?.ForEach(result.Add);
        }

        return result;
    }

    private async Task<Result<List<AmazonLookup>, Exception>> getLookupFor(string isbn)
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
                return new Exception(response.ErrorMessage);
            }
            else if (response.Content == null)
            {
                var message = "response.Content in AmazonAccess.GetDataByKeyWords was found null";
                return new Exception(message);
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
                        Title = item.Title,
                        URL = item.Url,
                        LastUsed = DateTime.Now,
                    });
                }
            }
        }
        catch (Exception ex)
        {
            return ex;
        }

        return result;
    }
}