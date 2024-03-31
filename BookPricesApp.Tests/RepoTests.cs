using BookPricesApp.Domain.Files;
using BookPricesApp.Repo;
using BookPricesApp.Repo.Migrations;

namespace BookPricesApp.Tests;

public class RepoTests
{
    [Fact]
    public void Test1()
    {
        var result = new MigrationStartup().InitDB();
        Assert.Null(result.Ex);
    }

    [Fact]
    public void Test2()
    {
        var db = new BookPriceRepo();
        var test = db.InsertAmazonLookup(new List<AmazonLookup>
        {
            new AmazonLookup
            {
                ISBN13 = "Test",
                ASIN = "Test",
                Error = "Test",
                LastUsed = "Test",
            }
        });
        var test2 = "";
    }

    [Fact]
    public void Test3()
    {
        var db = new BookPriceRepo();
        var test = db.GetAmazonLookup();
        var test2 = "";
    }
}
