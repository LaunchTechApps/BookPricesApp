using BookPricesApp.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Engine.Exchange;
public interface IExchangeEngine
{
    void Run(BookExchange exchange);
    public void Stop(BookExchange exchange);
}
