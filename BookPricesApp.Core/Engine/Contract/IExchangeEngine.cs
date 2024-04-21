using BookPricesApp.Core.Utils;

namespace BookPricesApp.Core.Engine.Contract;
public interface IExchangeEngine
{
    TResult<TVoid> Run(List<string> isbnList);
}
