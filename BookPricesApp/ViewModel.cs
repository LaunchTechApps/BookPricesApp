using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp;

public enum BookExchange
{
    Amazon,
    Ebay
}

internal class ViewModel
{
    private Dictionary<BookExchange, Panel> _panelDic = new();
    private Dictionary<BookExchange, Label> _tabDic = new();

    public ViewModel(
        Dictionary<BookExchange, Panel> paneDic, 
        Dictionary<BookExchange, Label> tabDic)
    {
        _panelDic = paneDic;
        _tabDic = tabDic;
    }

    public void SetActiveTab(BookExchange exchange)
    {
        _tabDic.Values.ToList().ForEach(t => t.BackColor = Color.FromName("Control"));
        _tabDic[exchange].BackColor = Color.FromName("ActiveBorder");
        _panelDic[exchange].BringToFront();
    }
}
