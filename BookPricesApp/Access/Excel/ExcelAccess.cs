using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Access.Excel;
public class ExcelAccess
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
        string filePath = "example.xlsx";

        // Use MiniExcel to save the DataTable to an Excel file
        MiniExcel.SaveAs(filePath, dt);
    }
}
