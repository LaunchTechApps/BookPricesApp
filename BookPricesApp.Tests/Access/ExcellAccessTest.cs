using BookPricesApp.Core.Access.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Tests.Access;
public class ExcellAccessTest
{
    [Fact]
    public void Test1()
    {
        new ExcelAccess().Run();
        Assert.True(true);
    }
}
