using BookPricesApp.Core.Access.FlatFile.Models;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System.Data;

namespace BookPricesApp.Core.Access.FlatFile;

public interface IFlatFileAccess
{
    Option OutputAppend(DataTable books);
}

public class FlatFileAccess : IFlatFileAccess
{
    private string _path = string.Empty;
    private bool _exporting = false;
    private readonly OpenXmlConfiguration _exportConfig = new OpenXmlConfiguration
    {
        TableStyles = TableStyles.None,
    };

    public FlatFileAccess(Config config)
    {
        _path = config.OutputPath;
    }

    public Option OutputAppend(DataTable books)
    {
        if (_exporting)
        {
            return new();
        }

        _exporting = true;

        try
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            MiniExcel.SaveAs(_path, books, configuration: _exportConfig);
        }
        catch (Exception ex)
        {
            return new Option { Ex = ex };
        }

        _exporting = false;
        return new();
    }
}