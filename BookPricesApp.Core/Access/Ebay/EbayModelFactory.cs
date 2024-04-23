using BookPricesApp.Core.Access.Ebay.Contract;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Access.Ebay;
public class EbayKeywordResponseProps
{
    public EbayKeyWordResponse? KeyWordResponse { get; set; }
    public string ISBN { get; set; } = string.Empty;
}

public class EbayModelFactory
{
    public List<ExportModel> CreateExportFrom(EbayKeywordResponseProps props)
    {
        var result = new List<ExportModel>();
        try
        {
            var keyWordResults = props?.KeyWordResponse?.FindItemsByKeywordsResponse?
                .FirstOrDefault()?.SearchResult?
                .FirstOrDefault()?.Item?
                .Where(i => isABook(i))
                .Where(i => i?.ListingInfo?.FirstOrDefault()?.ListingType?.FirstOrDefault()?.ToLower() == "fixedprice")
                .ToArray();

            if (keyWordResults is null)
            {
                result.Add(new ExportModel
                {
                    ISBN = props!.ISBN,
                    Source = "eBay",
                    Error = "No offers found"
                });
                return result;
            }

            foreach (var listing in keyWordResults)
            {
                if (props?.ISBN is null) { continue; }
                var model = new ExportModel
                {
                    ISBN = props.ISBN,
                    ItemId = listing?.ItemId?.FirstOrDefault() ?? "",
                    Title = listing?.Title?.FirstOrDefault() ?? "",
                    ItemUrl = listing?.ViewItemUrl?.FirstOrDefault() ?? "",
                    Condition = listing?.Condition?.FirstOrDefault()?.ConditionDisplayName?.FirstOrDefault() ?? "",
                    Seller = string.Empty, // not able to find the selling from the response
                    Location = listing?.Location?.FirstOrDefault() ?? "",
                    ShippingPrice =
                        listing?.ShippingInfo?.FirstOrDefault()?.ShippingServiceCost?.FirstOrDefault()?.__value__ + " " +
                        listing?.ShippingInfo?.FirstOrDefault()?.ShippingServiceCost?.FirstOrDefault()?.CurrencyId + " " +
                        listing?.ShippingInfo?.FirstOrDefault()?.ShippingType?.FirstOrDefault(),
                    Price = listing?.SellingStatus?.FirstOrDefault()?.ConvertedCurrentPrice?.FirstOrDefault()?.__value__,
                    Source = "eBay",
                };
                model.ShippingPrice = model.ShippingPrice.Trim();
                result.Add(model);
            }

            return result;
        }
        catch (Exception ex)
        {
            result.Add(new ExportModel
            {
                ISBN = props!.ISBN,
                Source = "eBay",
                Error = Error.From(ex).ToString()
            });
            return result;
        }
    }

    private bool isABook(Item i)
    {
        var primaryCategory = i?.PrimaryCategory?.FirstOrDefault();
        if (primaryCategory is null)
        {
            return false;
        }
        
        var categoryId = primaryCategory?.CategoryId?.FirstOrDefault() ?? "";
        categoryId = categoryId.Trim();
        if (new List<string> { "1105", "261186" }.Contains(categoryId))
        {
            return true;
        }
        
        var categoryName = primaryCategory?.CategoryName?.FirstOrDefault();
        categoryName = categoryName?.Trim().ToLower() ?? "";
        if (new List<string> { "books", "textbooks" }.Contains(categoryName))
        {
            return true;
        }

        if (categoryName.Contains("book") && !categoryName.Contains("audio"))
        {
            return true;
        }

        return false;
    }
}
