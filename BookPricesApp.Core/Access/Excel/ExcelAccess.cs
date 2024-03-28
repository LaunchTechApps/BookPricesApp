using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System.Data;

namespace BookPricesApp.Core.Access.Excel;

public interface IExcelAccess { }
public class ExcelAccess : IExcelAccess
{
    public void Run()
    {
        // Create a DataTable
        DataTable dt = new DataTable();
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Age", typeof(int));
        dt.Columns.Add("City", typeof(string));

        // Add some rows
        dt.Rows.Add("John Doe", 30, "New York");
        dt.Rows.Add("Jane Doe", 28, "Los Angeles");
        dt.Rows.Add("Jim Smith", 35, "Chicago");

        // Specify the path where you want to save the Excel file
        string filePath = "C:\\Users\\paulp\\Desktop\\example.xlsx";

        // Use MiniExcel to save the DataTable to an Excel file
        var config = new OpenXmlConfiguration()
        {
            TableStyles = TableStyles.None
        };
        MiniExcel.SaveAs(filePath, dt, configuration: config);
    }
}

