using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Domain;
public class ExportModel
{
    public string ISBN { get; set; } = string.Empty;
    public string? ItemId { get; set; }
    public string? Title { get; set; }
    public string? Seller { get; set; }
    public string? Location { get; set; }
    public string? ShippingPrice { get; set; }
    public string? Price { get; set; }
    public string? Condition { get; set; }
    public string? ItemUrl { get; set; }
    public string? Source { get; set; }
    public string? Error { get; set; }
}
