using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Utils;
using BookPricesApp.Domain.Files;
using MiniExcelLibs;

namespace BookPricesApp.Core.Access.FlatFile.Models;

public class AmazonLookupFile
{
    private readonly object _fileLock = new object();
    private string _folderPath
    {
        get
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var appData = Environment.GetFolderPath(folder);
            return $"{appData}\\{Global.AppName}Files";
        }
    }

    private string _filePath => $"{_folderPath}\\AmazonLookup.csv";

    public Option Append(List<AmazonLookup> books)
    {
        try
        {
            lock (_fileLock)
            {
                var amazonLookup = new List<AmazonLookup>();
                if (File.Exists(_filePath))
                {
                    var savedBooks = MiniExcel.Query<AmazonLookup>(_filePath, excelType: ExcelType.CSV);
                    amazonLookup.AddRange(savedBooks);
                }
                amazonLookup.AddRange(books);
                amazonLookup = amazonLookup.DistinctBy(b => b.ISBN13).ToList();

                if (!Directory.Exists(_folderPath))
                {
                    Directory.CreateDirectory(_folderPath);
                }

                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }

                MiniExcel.SaveAs(_filePath, amazonLookup, excelType: ExcelType.CSV);
            }
        }
        catch (Exception ex)
        {
            return new Option(ex);
        }
        return new();
    }

    public Option<List<AmazonLookup>> GetLookupList()
    {
        try
        {
            lock (_fileLock)
            {
                var data = new List<AmazonLookup>();
                foreach(var book in MiniExcel.Query<AmazonLookup>(_filePath, excelType: ExcelType.CSV))
                {
                    data.Add(new AmazonLookup
                    {
                        ISBN13 = book.ISBN13,
                        ASIN = book.ASIN,
                        LastUsed = book.LastUsed
                    });
                }
                return new Option<List<AmazonLookup>>()
                {
                    Data = data
                };
            }
        }
        catch (Exception ex)
        {
            return new Option<List<AmazonLookup>>(ex);
        }
    }
}
