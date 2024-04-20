using BookPricesApp.Core.Access.Amazon.Models;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;

namespace BookPricesApp.Domain.Export;

public class AmazonExportProps
{
    public AmazonLookup Lookup { get; set; } = new();
    public ProductOffers? Offers { get; set; }
}

public class AmazonLookupProps
{
    public ProductListing? Listing { get; set; }
    public string ISBN { get; set; } = string.Empty;
}

public class AmazonModelFactory
{
    public TResult<List<ExportModel>> CreateExport(AmazonExportProps props)
    {
        try
        {
            var result = new List<ExportModel>();
            var fo = props.Offers?.FeaturedOffer;
            var offerCount = 0;
            if (fo != null)
            {
                result.Add(new ExportModel
                {
                    ISBN = props.Lookup.ISBN13,
                    ItemId = props.Lookup.ASIN ?? "",
                    Title = props.Lookup.Title ?? "",
                    ItemUrl = props.Lookup.URL ?? "",
                    Condition = $"{fo.Condition.ConditionValue}-{fo.Condition.SubCondition}",
                    Seller = fo.Merchant.Name ?? "",
                    Location = "US",
                    ShippingPrice = fo.DeliveryInformation ?? "",
                    Price = fo.Price.Value.Amount.ToString(),
                    Source = "Amazon",
                });
                offerCount++;
            }

            foreach (var item in props.Offers?.Offers ?? Array.Empty<Offer>())
            {
                result.Add(new ExportModel
                {
                    ISBN = props.Lookup.ISBN13,
                    ItemId = props.Lookup.ASIN ?? "",
                    Title = props.Lookup.Title ?? "",
                    ItemUrl = props.Lookup.URL ?? "",
                    Condition = $"{item.Condition.ConditionValue}-{item.Condition.SubCondition}",
                    Seller = item.Merchant.Name ?? "",
                    Location = "US",
                    ShippingPrice = item.DeliveryInformation ?? "",
                    Price = item.Price.Value.Amount.ToString(),
                    Source = "Amazon",
                });
                offerCount++;
            }

            if (offerCount == 0)
            {
                result.Add(new ExportModel
                {
                    ISBN = props.Lookup.ISBN13,
                    Source = "Amazon",
                    Error = "No offers found"
                });
            }
            return result;
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
    }

    public TResult<List<AmazonLookup>> CreateLookup(AmazonLookupProps props)
    {
        try
        {
            var result = new List<AmazonLookup>();

            if (props.Listing?.Products?.Length == 0)
            {
                result.Add(new AmazonLookup
                {
                    ISBN13 = props.ISBN,
                    LastUsed = DateTime.Now,
                    Error = $"No products found in keyword search"
                });
            }
            foreach (var item in props.Listing?.Products ?? Array.Empty<Product>())
            {
                result.Add(new AmazonLookup
                {
                    ISBN13 = props.ISBN,
                    ASIN = item.Asin,
                    Title = item.Title,
                    URL = item.Url,
                    LastUsed = DateTime.Now,
                });
            }

            return result;
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
    }
}
