using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Domain;
public class BookLookup
{
    public string Exchange { get; set; } = string.Empty;
    public string ISBN13 { get; set; } = string.Empty;
    public string? ExchangeId { get; set; }
    public string? LastUsed { get; set; }
    public bool HasLookup => ExchangeId != null;
}
