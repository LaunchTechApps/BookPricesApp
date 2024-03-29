using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Access.Amazon.Models;

public class ProductListing
{
    public int matchingProductCount { get; set; }
    public Searchrefinement[] searchRefinements { get; set; }
    public Refinements refinements { get; set; }
    public int numberOfPages { get; set; }
    public Product[] products { get; set; }
}

public class Refinements
{
    public Category[] categories { get; set; }
    public object[] subCategories { get; set; }
    public Availabilityoption[] availabilityOptions { get; set; }
    public object[] deliveryDayOptions { get; set; }
    public object[] eligibleForFreeShippingOptions { get; set; }
    public object[] primeEligible { get; set; }
}

public class Category
{
    public string displayName { get; set; }
    public string id { get; set; }
}

public class Availabilityoption
{
    public string displayName { get; set; }
    public string id { get; set; }
}

public class Searchrefinement
{
    public string selectionType { get; set; }
    public string displayValue { get; set; }
    public Refinementvalue[] refinementValues { get; set; }
}

public class Refinementvalue
{
    public string displayName { get; set; }
    public string searchRefinementValue { get; set; }
}

public class Product
{
    public string asin { get; set; }
    public string asinType { get; set; }
    public string signedProductId { get; set; }
    public Includeddatatypes includedDataTypes { get; set; }
    public object[] features { get; set; }
    public object[] editorialReviews { get; set; }
    public Taxonomy[] taxonomies { get; set; }
    public string title { get; set; }
    public string url { get; set; }
    public object format { get; set; }
    public Bookinformation bookInformation { get; set; }
    public object[] byLine { get; set; }
    public Mediainformation mediaInformation { get; set; }
    public Productoverview productOverview { get; set; }
    public Productdetails productDetails { get; set; }
    public Productvariations productVariations { get; set; }
    public Customerreviewssummary customerReviewsSummary { get; set; }
    public object productDescription { get; set; }
    public object[] upcValues { get; set; }
    public object[] eanValues { get; set; }
}

public class Includeddatatypes
{
    public object[] OFFERS { get; set; }
}

public class Bookinformation
{
    public Isbn isbn { get; set; }
    public object publicationDate { get; set; }
    public object publishedLanguage { get; set; }
}

public class Isbn
{
    public object isbn10 { get; set; }
    public object isbn13 { get; set; }
}

public class Mediainformation
{
    public object[] editions { get; set; }
    public object[] mediaFormats { get; set; }
}

public class Productoverview
{
}

public class Productdetails
{
}

public class Productvariations
{
    public object[] dimensions { get; set; }
    public object[] variations { get; set; }
}

public class Customerreviewssummary
{
    public object numberOfReviews { get; set; }
    public object starRating { get; set; }
}

public class Taxonomy
{
    public string taxonomyCode { get; set; }
    public string title { get; set; }
    public string type { get; set; }
}

