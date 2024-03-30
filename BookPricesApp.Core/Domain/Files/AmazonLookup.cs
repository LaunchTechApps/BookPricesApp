using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Domain;
public class AmazonLookup
{
    public string ISBN13 { get; set; } = string.Empty;
    public string? ASIN { get; set; }
    public string? LastUsed { get; set; }
    public string? Error { get; set; }
    public bool HasLookup() => ASIN != null;
}
