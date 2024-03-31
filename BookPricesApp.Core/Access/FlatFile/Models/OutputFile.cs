using BookPricesApp.Core.Utils;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System;
using System.Data;

namespace BookPricesApp.Core.Access.FlatFile.Models;

//public class OutputFile
//{
//    private string _path = string.Empty;

//    private readonly OpenXmlConfiguration _exportConfig = new OpenXmlConfiguration
//    {
//        TableStyles = TableStyles.None,
//    };

//    public void CreateNewExport(string path)
//    {
//        _path = path;
//    }

//    private bool _exporting = false;
//    public Result<int, Exception> Append(DataTable books)
//    {
//        if (_exporting)
//        {
//            return new();
//        }

//        _exporting = true;

//        try
//        {
//            if (File.Exists(_path))
//            {
//                File.Delete(_path);
//            }

//            MiniExcel.SaveAs(_path, books, configuration: _exportConfig);
//        }
//        catch (Exception ex)
//        {
//            return ex;
//        }

//        _exporting = false;
//        return 0;
//    }
//}