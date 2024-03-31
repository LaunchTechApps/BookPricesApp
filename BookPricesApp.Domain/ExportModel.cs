using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Domain;
public class ExportModel
{
    public string ISBN { get; set; } = string.Empty;
    public string ItemId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Seller { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string ShippingPrice { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string ItemUrl { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
