using BookPricesApp.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace BookPricesApp.Core.Access.Ebay.Models;
public class ApiKeysModel
{
    private List<string> _apiKeys = new();
    private int _idx;
    public ApiKeysModel(IConfiguration config)
    {
        _apiKeys = config.GetSection("ebay:apiKeys")
            .GetChildren()
            .Select(c => c.Value?.Trim() ?? "")
            .Where(key => !string.IsNullOrEmpty(key))
            .ToList();
    }

    public TResult<string> GetNextApiKey()
    {
        try
        {
            if (_apiKeys.Count == 0)
            {
                return EBayAccessError.EmptyApiKeys;
            }
            if (_idx == _apiKeys.Count)
            {
                return EBayAccessError.RantOutOfKeys;
            }

            var nextKey = _apiKeys[_idx];

            _idx++;

            return nextKey;
        }
        catch (Exception ex)
        {
            return Error.From(ex);
        }
    }
}
