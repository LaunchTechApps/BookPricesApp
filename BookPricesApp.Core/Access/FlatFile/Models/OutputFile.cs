using BookPricesApp.Core.Utils;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System;
using System.Data;

namespace BookPricesApp.Core.Access.FlatFile.Models;

public class OutputFile
{
    private string _path = string.Empty;
    private readonly OpenXmlConfiguration _exportConfig = new OpenXmlConfiguration
    {
        TableStyles = TableStyles.None,
    };

    public Option CreateNewExport(string path)
    {
        _path = path;
        return new();
    }

    private bool _exporting = false;
    public Option Append(DataTable books)
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