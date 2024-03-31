using BookPricesApp.Core.Access.Amazon.Models;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using BookPricesApp.Domain.Files;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BookPricesApp.Core.Access.Amazon;

public interface IAmazonAccess { }
public class AmazonAccess : IAmazonAccess
{
    private AmazonConfig _config;
    private string _email;
    private EventBus _bus;
    private string? _accessToken;

    public AmazonAccess(Config config, EventBus bus)
    {
        _config = config.Amazon;
        _bus = bus;
        _email = config.Email;
    }

    public async Task<Result<int, Exception>> SetRefresToken()
    {
        try
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

    public async Task<Result<List<ExportModel>, Exception>> GetDataByLookup(List<AmazonLookup> lookups)
    {
        var result = new List<ExportModel>();
        try
        {
            var progress = new ProgressCounter(lookups.Count);
            foreach (var lookup in lookups)
            {
                var exports = await getExportFor(lookup);
                if (exports.Error != null)
                {
                    // what do we do here?
                    var test = "";
                }
                else if (exports.Value != null)
                {
                    result.AddRange(exports.Value);
                }
                _bus.Publish(new ProgressEvent
                {
                    Percent = progress.Increment(),
                    Exchange = BookExchange.Amazon
                });
            }
            result = result.Select(s => { s.Source = "Amazon"; return s; }).ToList();
        }
        catch (Exception ex)
        {
            return ex;
        }

        return result;
    }

    public async Task<Result<List<AmazonLookup>, Exception>> GetLookupsFor(List<string> isbnList)
    {
        if (_accessToken == null)
        {
            var ex = new Exception("_accessToken was found null");
            return ex;
        }

        var result = new List<AmazonLookup>();
        var progress = new ProgressCounter(isbnList.Count);
        foreach (var isbn in isbnList)
        {
            var lookup = await getLookupFor(isbn);

            if (lookup.Error != null)
            {
                result.Add(new AmazonLookup
                {
                    ISBN13 = isbn,
                    LastUsed = DateTime.UtcNow.ToIso8601(),
                    Error = lookup.Error.Message
                });
            }
            else
            {
                lookup.Value?.ForEach(result.Add);
            }
            
            _bus.Publish(new ProgressEvent
            {
                Percent = progress.Increment(),
                Exchange = BookExchange.Amazon
            });
        }

        return result;
    }
    private async Task<Result<List<ExportModel>, Exception>> getExportFor(AmazonLookup lookup)
    {
        var result = new List<ExportModel>();
        try
        {
            var path = "/products/2020-08-26/products";
            var query = $"/{lookup.ASIN}/offers?locale=en_US&productRegion=US";
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
                        Price =fo.Price.Value.Amount.ToString(),
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

    private async Task<Result<List<AmazonLookup>, Exception>> getLookupFor(string isbn)
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
                        LastUsed = DateTime.Now.ToIso8601(),
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