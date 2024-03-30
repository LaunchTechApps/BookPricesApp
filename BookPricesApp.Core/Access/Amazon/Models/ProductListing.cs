using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Access.Amazon.Models;

public class ProductListing
{
    public int MatchingProductCount { get; set; }
    public SearchRefinement[]? SearchRefinements { get; set; }
    public Refinements? Refinements { get; set; }
    public int numberOfPages { get; set; }
    public Product[]? Products { get; set; }
}

public class Refinements
{
    public Category[]? Categories { get; set; }
    public object[]? SubCategories { get; set; }
    public AvailabilityOption[]? AvailabilityOptions { get; set; }
    public object[]? DeliveryDayOptions { get; set; }
    public object[]? EligibleForFreeShippingOptions { get; set; }
    public object[]? PrimeEligible { get; set; }
}

public class Category
{
    public string? DisplayName { get; set; }
    public string? Id { get; set; }
}

public class AvailabilityOption
{
    public string? DisplayName { get; set; }
    public string? Id { get; set; }
}

public class SearchRefinement
{
    public string? SelectionType { get; set; }
    public string? DisplayValue { get; set; }
    public RefinementValue[]? RefinementValues { get; set; }
}

public class RefinementValue
{
    public string? DisplayName { get; set; }
    public string? SearchRefinementValue { get; set; }
}

public class Product
{
    public string? Asin { get; set; }
    public string? AsinType { get; set; }
    public string? SignedProductId { get; set; }
    public IncludedDataTypes? IncludedDataTypes { get; set; }
    public object[]? Features { get; set; }
    public object[]? EditorialReviews { get; set; }
    public Taxonomy[]? Taxonomies { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public object? Format { get; set; }
    public BookInformation? BookInformation { get; set; }
    public object[]? ByLine { get; set; }
    public MediaInformation? MediaInformation { get; set; }
    public ProductOverview? ProductOverview { get; set; }
    public ProductDetails? ProductDetails { get; set; }
    public ProductVariations? ProductVariations { get; set; }
    public CustomerReviewsSummary? CustomerReviewsSummary { get; set; }
    public object? ProductDescription { get; set; }
    public object[]? UpcValues { get; set; }
    public object[]? EanValues { get; set; }
}

public class IncludedDataTypes
{
    public object[]? OFFERS { get; set; }
}

public class BookInformation
{
    public Isbn? Isbn { get; set; }
    public object? PublicationDate { get; set; }
    public object? PublishedLanguage { get; set; }
}

public class Isbn
{
    public object? Isbn10 { get; set; }
    public object? Isbn13 { get; set; }
}

public class MediaInformation
{
    public object[]? Editions { get; set; }
    public object[]? MediaFormats { get; set; }
}

public class ProductOverview
{
}

public class ProductDetails
{
}

public class ProductVariations
{
    public object[]? Dimensions { get; set; }
    public object[]? Variations { get; set; }
}

public class CustomerReviewsSummary
{
    public object? NumberOfReviews { get; set; }
    public object? StarRating { get; set; }
}

public class Taxonomy
{
    public string? TaxonomyCode { get; set; }
    public string? Title { get; set; }
    public string? Type { get; set; }
}
