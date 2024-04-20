using BookPricesApp.Domain.Export;

namespace BookPricesApp.Core.Test;

public class ExportFactoryTest
{
    [Fact]
    public void When_We_Have_Correct_Data_No_Errors_Should_Occur()
    {
        var objectFactory = new ObjectFactory();
        var exportFactory = new AmazonModelFactory();

        var offers = objectFactory.CreateAmazonProductOffers();
        var lookup = objectFactory.CreateAmazonLookup();
        var exportResult = exportFactory.CreateExport(new AmazonExportProps
        {
            Offers = offers,
            Lookup = lookup
        });

        Assert.False(exportResult.DidError);
    }
}