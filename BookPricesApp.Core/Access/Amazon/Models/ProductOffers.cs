public class ProductOffers
{
    public int OfferCount { get; set; }
    public int NumberOfPages { get; set; }
    public FeaturedOffer? FeaturedOffer { get; set; }
    public Offer[]? Offers { get; set; }
    public FilterGroup[]? FilterGroups { get; set; }
}

public class FeaturedOffer
{
    public string? Availability { get; set; }
    public string? BuyingGuidance { get; set; }
    public object[]? BuyingRestrictions { get; set; }
    public string? FulfillmentType { get; set; }
    public Merchant? Merchant { get; set; }
    public string? OfferId { get; set; }
    public Price? Price { get; set; }
    public object[]? Badges { get; set; }
    public object? QuantityInStock { get; set; }
    public ListPrice? ListPrice { get; set; }
    public string? ProductCondition { get; set; }
    public string? ProductConditionNote { get; set; }
    public Condition? Condition { get; set; }
    public QuantityLimits? QuantityLimits { get; set; }
    public QuantityPrice? QuantityPrice { get; set; }
    public TaxExclusivePrice? TaxExclusivePrice { get; set; }
    public string? DeliveryInformation { get; set; }
}

public class Merchant
{
    public object? MerchantId { get; set; }
    public string? Name { get; set; }
    public object? MeanFeedbackRating { get; set; }
    public object? TotalFeedbackCount { get; set; }
}

public class Price
{
    public Value? Value { get; set; }
    public object? FormattedPrice { get; set; }
    public string? PriceType { get; set; }
}

public class Value
{
    public float? Amount { get; set; }
    public string? CurrencyCode { get; set; }
}

public class ListPrice
{
    public object? Value { get; set; }
    public object? FormattedPrice { get; set; }
    public object? PriceType { get; set; }
}

public class Condition
{
    public string? ConditionValue { get; set; }
    public string? ConditionNote { get; set; }
    public string? SubCondition { get; set; }
}

public class QuantityLimits
{
    public int MaxQuantity { get; set; }
    public int MinQuantity { get; set; }
}

public class QuantityPrice
{
    public object[]? QuantityPriceTiers { get; set; }
}

public class TaxExclusivePrice
{
    public object? TaxExclusiveAmount { get; set; }
    public string? DisplayString { get; set; }
    public string? FormattedPrice { get; set; }
    public string? Label { get; set; }
}

public class Offer
{
    public string? Availability { get; set; }
    public string? BuyingGuidance { get; set; }
    public object[]? BuyingRestrictions { get; set; }
    public string? FulfillmentType { get; set; }
    public Merchant1? Merchant { get; set; }
    public string? OfferId { get; set; }
    public Price1? Price { get; set; }
    public object[]? Badges { get; set; }
    public object? QuantityInStock { get; set; }
    public ListPrice1? ListPrice { get; set; }
    public string? ProductCondition { get; set; }
    public string? ProductConditionNote { get; set; }
    public Condition1? Condition { get; set; }
    public QuantityLimits1? QuantityLimits { get; set; }
    public QuantityPrice1? QuantityPrice { get; set; }
    public TaxExclusivePrice1? TaxExclusivePrice { get; set; }
    public string? DeliveryInformation { get; set; }
}

public class Merchant1
{
    public object? MerchantId { get; set; }
    public string? Name { get; set; }
    public object? MeanFeedbackRating { get; set; }
    public object? TotalFeedbackCount { get; set; }
}

public class Price1
{
    public Value1? Value { get; set; }
    public object? FormattedPrice { get; set; }
    public string? PriceType { get; set; }
}

public class Value1
{
    public float Amount { get; set; }
    public string? CurrencyCode { get; set; }
}

public class ListPrice1
{
    public object? Value { get; set; }
    public object? FormattedPrice { get; set; }
    public object? PriceType { get; set; }
}

public class Condition1
{
    public string? ConditionValue { get; set; }
    public string? ConditionNote { get; set; }
    public string? SubCondition { get; set; }
}

public class QuantityLimits1
{
    public int MaxQuantity { get; set; }
    public int MinQuantity { get; set; }
}

public class QuantityPrice1
{
    public object[]? QuantityPriceTiers { get; set; }
}

public class TaxExclusivePrice1
{
    public object? TaxExclusiveAmount { get; set; }
    public string? DisplayString { get; set; }
    public string? FormattedPrice { get; set; }
    public string? Label { get; set; }
}

public class FilterGroup
{
    public string? DisplayName { get; set; }
    public Filter[]? Filters { get; set; }
}

public class Filter
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
}