using BookPricesApp.Access.Amazon;
using BookPricesApp.Domain.Types;
using BookPricesApp.Engine.Exchange;
using BookPricesApp.Manager.App;
using BookPricesApp.Utils.Config;
using BookPricesApp.Utils;
using Microsoft.Extensions.DependencyInjection;
using BookPricesApp.Manager.Display.Models;

namespace BookPricesApp.Manager.Display;
public class DisplayManager
{
    private Dictionary<BookExchange, Panel> _panelDic = new();
    private Dictionary<BookExchange, Label> _tabDic = new();
    private List<SelectGroup> _selectGroup = new();
    private ServiceProvider _serviceProvider;
    public DisplayManager(Dictionary<BookExchange, Panel> paneDic,
        Dictionary<BookExchange, Label> tabDic,
        List<SelectGroup> selectGroup)
    {
        _panelDic = paneDic;
        _tabDic = tabDic;
        _selectGroup = selectGroup;

        var services = new ServiceCollection();
        services.AddSingleton(new AppConfig());
        services.AddSingleton<IAppManager, AppManager>();
        services.AddSingleton<IExchangeEngine, ExchangeEngine>();
        services.AddSingleton<IAmazonAccess, AmazonAccess>();
        services.AddSingleton(new EventBus());

        _serviceProvider = services.BuildServiceProvider();
    }
    public void SetActiveTab(BookExchange exchange)
    {
        _tabDic.Values.ToList().ForEach(t => t.BackColor = Color.FromName("Control"));
        _tabDic[exchange].BackColor = Color.FromName("ActiveBorder");
        _panelDic[exchange].BringToFront();
    }

    internal void FilePickerSelect(OpenFileDialog filePicker, BookExchange exchange)
    {
        var group = _selectGroup.FirstOrDefault(g => Equals(exchange, g.Exchange));
        if (group == null || group.SelectTextBox == null)
        {
            MessageBox.Show("unable to find " + exchange);
            return;
        }

        filePicker.Filter = "Text|*.txt|All|*.*";
        var selected = filePicker.ShowDialog();
        if (selected == DialogResult.OK)
        {
            var fileName = filePicker.FileName;
            group.SelectTextBox.Text = fileName;
        }
    }

    public void Start(BookExchange exchange)
    {
        switch (exchange)
        {
            case BookExchange.Amazon:
                _serviceProvider.GetService<IExchangeEngine>()?.Run(BookExchange.Amazon);
                break;
            default:
                MessageBox.Show("Unable to find process for " + exchange);
                break;
        }
    }

    public void Stop(BookExchange exchange)
    {
        switch (exchange)
        {
            case BookExchange.Amazon:
                _serviceProvider.GetService<IExchangeEngine>()?.Stop(BookExchange.Amazon);

                break;
            default:
                MessageBox.Show("Unable to find process for " + exchange);
                break;
        }
    }
}
