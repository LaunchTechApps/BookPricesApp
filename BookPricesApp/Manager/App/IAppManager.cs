using BookPricesApp.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Manager.App;
public interface IAppManager
{
    void SubmitMainEvent(BookExchange exchange);
}
