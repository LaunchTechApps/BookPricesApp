using BookPricesApp.Core.Access.DB;
using BookPricesApp.Domain.Files;

namespace BookPricesApp.Tests;

public class RepoTests
{
    [Fact]
    public void Test1()
    {
        var result = new DBAccess().InitDB();
        Assert.Null(result.Error);
    }

    [Fact]
    public void Test2()
    {
        var db = new DBAccess();
        var result = db.InsertAmazonLookup(new List<AmazonLookup>
        {
            new AmazonLookup
            {
                ISBN13 = "Test",
                ASIN = "Test",
                Error = "Test",
                LastUsed = DateTime.Now,
            }
        });
        Assert.Null(result.Error);
    }

    [Fact]
    public void Test3()
    {
        var db = new DBAccess();
        var result = db.GetAmazonLookup();
        Assert.Null(result.Error);
    }
}
