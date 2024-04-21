namespace BookPricesApp.Core.Access.Ebay.Contract;
public class EbayKeyWordResponse
{
    public FindItemsByKeywordsResponse[]? FindItemsByKeywordsResponse { get; set; }
}

public class FindItemsByKeywordsResponse
{
    public string[]? Ack { get; set; }
    public string[]? Version { get; set; }
    public DateTime[]? Timestamp { get; set; }
    public SearchResult[]? SearchResult { get; set; }
    public PaginationOutput[]? PaginationOutput { get; set; }
    public string[]? ItemSearchUrl { get; set; }
}

public class SearchResult
{
    public string? Count { get; set; }
    public Item[]? Item { get; set; }
}

public class Item
{
    public string[]? ItemId { get; set; }
    public string[]? Title { get; set; }
    public string[]? GlobalId { get; set; }
    public PrimaryCategory[]? PrimaryCategory { get; set; }
    public string[]? GalleryUrl { get; set; }
    public string[]? ViewItemUrl { get; set; }
    public string[]? AutoPay { get; set; }
    public string[]? PostalCode { get; set; }
    public string[]? Location { get; set; }
    public string[]? Country { get; set; }
    public ShippingInfo[]? ShippingInfo { get; set; }
    public SellingStatus[]? SellingStatus { get; set; }
    public ListingInfo[]? ListingInfo { get; set; }
    public string[]? ReturnsAccepted { get; set; }
    public Condition[]? Condition { get; set; }
    public string[]? IsMultiVariationListing { get; set; }
    public string[]? TopRatedListing { get; set; }
    public ProductId[]? ProductId { get; set; }
    public string[]? Subtitle { get; set; }
    public string[]? CharityId { get; set; }
    public DiscountPriceInfo[]? DiscountPriceInfo { get; set; }
    public string[]? PaymentMethod { get; set; }
}

public class PrimaryCategory
{
    public string[]? CategoryId { get; set; }
    public string[]? CategoryName { get; set; }
}

public class ShippingInfo
{
    public ShippingServiceCost[]? ShippingServiceCost { get; set; }
    public string[]? ShippingType { get; set; }
    public string[]? ShipToLocations { get; set; }
    public string[]? ExpeditedShipping { get; set; }
    public string[]? OneDayShippingAvailable { get; set; }
    public string[]? HandlingTime { get; set; }
}

public class ShippingServiceCost
{
    public string? CurrencyId { get; set; }
    public string? __value__ { get; set; }
}

public class SellingStatus
{
    public CurrentPrice[]? CurrentPrice { get; set; }
    public ConvertedCurrentPrice[]? ConvertedCurrentPrice { get; set; }
    public string[]? BidCount { get; set; }
    public string[]? SellingState { get; set; }
    public string[]? TimeLeft { get; set; }
}

public class CurrentPrice
{
    public string? CurrencyId { get; set; }
    public string? __value__ { get; set; }
}

public class ConvertedCurrentPrice
{
    public string? CurrencyId { get; set; }
    public string? __value__ { get; set; }
}

public class ListingInfo
{
    public string[]? BestOfferEnabled { get; set; }
    public string[]? BuyItNowAvailable { get; set; }
    public DateTime[]? StartTime { get; set; }
    public DateTime[]? EndTime { get; set; }
    public string[]? ListingType { get; set; }
    public string[]? Gift { get; set; }
    public string[]? WatchCount { get; set; }
    public BuyItNowPrice[]? BuyItNowPrice { get; set; }
    public ConvertedBuyItNowPrice[]? ConvertedBuyItNowPrice { get; set; }
}

public class BuyItNowPrice
{
    public string? CurrencyId { get; set; }
    public string? __value__ { get; set; }
}

public class ConvertedBuyItNowPrice
{
    public string? CurrencyId { get; set; }
    public string? __value__ { get; set; }
}

public class Condition
{
    public string[]? ConditionId { get; set; }
    public string[]? ConditionDisplayName { get; set; }
}

public class ProductId
{
    public string? Type { get; set; }
    public string? __value__ { get; set; }
}

public class DiscountPriceInfo
{
    public OriginalRetailPrice[]? OriginalRetailPrice { get; set; }
    public string[]? PricingTreatment { get; set; }
    public string[]? SoldOnEbay { get; set; }
    public string[]? SoldOffEbay { get; set; }
}

public class OriginalRetailPrice
{
    public string? CurrencyId { get; set; }
    public string? __value__ { get; set; }
}

public class PaginationOutput
{
    public string[]? PageNumber { get; set; }
    public string[]? EntriesPerPage { get; set; }
    public string[]? TotalPages { get; set; }
    public string[]? TotalEntries { get; set; }
}
