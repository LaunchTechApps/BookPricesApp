using BookPricesApp.Core.Access.Amazon.Models;

namespace BookPricesApp.Core.Test.Amazon;
public class AmazonAccessTest
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
