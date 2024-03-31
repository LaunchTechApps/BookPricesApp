using BookPricesApp.Core.Access.FlatFile.Models;
using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using BookPricesApp.Domain.Files;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System.Data;

namespace BookPricesApp.Core.Access.FlatFile;

public interface IFlatFileAccess
{
    Result<int, Exception> OutputAppend(List<ExportModel> exportList);
    void CreateNewExport();
}

public class FlatFileAccess : IFlatFileAccess
{
    private string _path = string.Empty;
    private bool _exporting = false;
    private readonly OpenXmlConfiguration _exportConfig = new OpenXmlConfiguration
    {
        TableStyles = TableStyles.None,
    };


    public void CreateNewExport()
    {
        var filename = $"output_{DateTime.Now.ToUnixSecondsStr()}.xlsx";
        _path = $"{Config.DBFolderPath}\\{filename}";
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
    }

    public Result<int, Exception> OutputAppend(List<ExportModel> exportList)
    {
        if (_exporting)
        {
            return new Exception("export already running");
        }

        _exporting = true;

        // TODO: need to get data from the old export and append it.

        try
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            MiniExcel.SaveAs(_path, exportList, configuration: _exportConfig);
        }
        catch (Exception ex)
        {
            return ex;
        }

        _exporting = false;
        return 0;
    }

    private Result<DataTable, Exception> getDataTableFrom(List<ExportModel> exports)
    {
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(COL.ISBN, typeof(string));
            dt.Columns.Add(COL.ItemId, typeof(string));
            dt.Columns.Add(COL.Title, typeof(string));
            dt.Columns.Add(COL.Seller, typeof(string));
            dt.Columns.Add(COL.Location, typeof(string));
            dt.Columns.Add(COL.ShippingPrice, typeof(string));
            dt.Columns.Add(COL.Price, typeof(string));
            dt.Columns.Add(COL.Condition, typeof(string));
            dt.Columns.Add(COL.ItemURL, typeof(string));
            dt.Columns.Add(COL.Source, typeof(string));

            for (var i = 0; i < exports.Count; i++)
            {
                DataRow row = dt.NewRow();

                row[COL.ISBN] = exports[i].ISBN ?? "";
                row[COL.ItemId] = exports[i].ItemId ?? "";
                row[COL.Title] = exports[i].Title ?? "";
                row[COL.Seller] = exports[i].Seller ?? "";
                row[COL.Location] = exports[i].Location ?? "";
                row[COL.ShippingPrice] = exports[i].ShippingPrice ?? "";
                row[COL.Price] = exports[i].Price ?? "";
                row[COL.Condition] = exports[i].Condition ?? "";
                row[COL.ItemURL] = exports[i].ItemId ?? "";
                row[COL.Source] = exports[i].Source ?? "";

                dt.Rows.Add(row);
            }

            return dt;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}