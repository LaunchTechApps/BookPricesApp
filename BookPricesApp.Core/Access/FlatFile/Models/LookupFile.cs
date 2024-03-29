using BookPricesApp.Core.Domain;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Utils;
using MiniExcelLibs;

namespace BookPricesApp.Core.Access.FlatFile.Models;

public class LookupFile
{
    private readonly object _fileLock = new object();
    public string _path { get; set; } = string.Empty;
    public LookupFile(string path)
    {
        _path = path;
    }

    internal Option Append(List<BookLookup> books)
    {
        try
        {
            lock (_fileLock)
            {
                var savedBooks = MiniExcel.Query<BookLookup>(_path, excelType: ExcelType.CSV);
                foreach (var book in books)
                {
                    savedBooks.Append(book);
                }

                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }
                MiniExcel.SaveAs(_path, savedBooks, excelType: ExcelType.CSV);
            }
        }
        catch (Exception ex)
        {
            return new Option(ex);
        }
        return new();
    }

    internal Option<List<BookLookup>> GetLookupListFor(BookExchange exchange)
    {
        try
        {
            lock (_fileLock)
            {
                var data = new List<BookLookup>();
                foreach(var book in MiniExcel.Query<BookLookup>(_path, excelType: ExcelType.CSV))
                {
                    if (book.Exchange.ToLower() != exchange.ToString().ToLower())
                    {
                        continue;
                    }
                    data.Add(new BookLookup
                    {
                        Exchange = book.Exchange,
                        ISBN13 = book.ISBN13,
                        ExchangeId = book.ExchangeId,
                        LastUsed = book.LastUsed
                    });
                }
                return new Option<List<BookLookup>>()
                {
                    Data = data
                };
            }
        }
        catch (Exception ex)
        {
            return new Option<List<BookLookup>>(ex);
        }
    }
}
