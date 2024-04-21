using Microsoft.Extensions.DependencyInjection;
using BookPricesApp.GUI.Models;
using BookPricesApp.GUI.Utils;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Manager.App;
using BookPricesApp.Core.Engine;
using BookPricesApp.Core.Access.Amazon;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Utils;
using BookPricesApp.Core.Access.DB;
using Microsoft.Extensions.Configuration;
using BookPricesApp.Core.Access.Ebay;

namespace BookPricesApp.GUI;

public class ViewModel
{
    private List<SelectGroup> _selectGroup = new();
    private ServiceProvider? _serviceProvider;
    private BookExchange _activeExchange = BookExchange.Amazon;
    private EventBus _bus = new();
    private DBAccess _db;
    private IConfiguration _config;
    public ViewModel(List<SelectGroup> selectGroup)
    {
        _selectGroup = selectGroup;

        _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var dbResult = new DBAccess(_config, _bus).InitDB();
        if (dbResult.DidError)
        {
            var message = $"Unable to init DBAccess: {dbResult.Error}";
            MessageBox.Show(message);
            Environment.Exit(1);
        }
        _db = dbResult.Value;

        var services = new ServiceCollection();

        services.AddSingleton(_bus);
        services.AddSingleton(_config);
        services.AddSingleton(_db);
        services.AddSingleton<IAppManager, AppManager>();
        services.AddSingleton<AmazonEngine>();
        services.AddSingleton<EbayEngine>();
        services.AddSingleton<EngineProvider>();
        services.AddSingleton<AmazonAccess>();
        services.AddSingleton<EbayAccess>();

        _serviceProvider = services.BuildServiceProvider();

        setupEventBus();
        setupSavedFilePaths();
    }

    public void SetActiveTab(BookExchange exchange)
    {
        _activeExchange = exchange;
        _selectGroup.ForEach(g =>
        {
            if (g.Tab != null)
            {
                g.Tab.BackColor = Color.FromName("Control");
            }
        });

        var group = _selectGroup.First(g => g.Exchange == exchange);
        if (group.Tab != null && group.Panel != null)
        {
            group.Tab.BackColor = Color.FromName("ActiveBorder");
            group.Tab.BringToFront();
            group.Panel.BringToFront();
        }
    }

    public void FilePickerSelect(OpenFileDialog filePicker)
    {
        var group = _selectGroup.FirstOrDefault(g => _activeExchange == g.Exchange);
        if (group == null || group.SelectTextBox == null)
        {
            MessageBoxQueue.Add($"unable to find {_activeExchange}");
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

    public void SubmitMainButton()
    {
        var group = _selectGroup.First(g => _activeExchange == g.Exchange);
        string isbnFilePath = string.Empty;
        if (group.SelectTextBox != null && string.IsNullOrEmpty(group.SelectTextBox.Text))
        {
            MessageBoxQueue.Add($"No file selected for {group.Exchange}");
            return;
        }
        else
        {
            isbnFilePath = group.SelectTextBox?.Text.Trim() ?? "";
        }

        var appManager = _serviceProvider?.GetService<IAppManager>();
        appManager?.SubmitMainEvent(_activeExchange, isbnFilePath);
    }

    private void setupSavedFilePaths()
    {
        var amazonGroup = _selectGroup.FirstOrDefault(g => g.Exchange == BookExchange.Amazon);
        if (amazonGroup != null)
        {
            if (amazonGroup.SelectTextBox != null && string.IsNullOrEmpty(amazonGroup.SelectTextBox.Text))
            {
                amazonGroup.SelectTextBox.Text = _db.GetIsbnFilePath(BookExchange.Amazon).Value;
                amazonGroup.SelectTextBox.Refresh();
            }
        }

        var ebayGroup = _selectGroup.FirstOrDefault(g => g.Exchange == BookExchange.Ebay);
        if (ebayGroup != null)
        {
            if (ebayGroup.SelectTextBox != null && string.IsNullOrEmpty(ebayGroup.SelectTextBox.Text))
            {
                ebayGroup.SelectTextBox.Text = _db.GetIsbnFilePath(BookExchange.Ebay).Value;
                ebayGroup.SelectTextBox.Refresh();
            }
        }
    }

    private void setupEventBus()
    {
        _bus?.OnEvent((ProgressEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.ProgressBar != null)
            {
                group.ProgressBar.Call(p => p.Value = e.Percent);
            }
        });

        _bus?.OnEvent((StartEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.MainButton != null)
            {
                group.MainButton.Call(p => p.Text = "Stop");
                group.MainButton.Call(p => p.BackColor = Color.FromArgb(255, 192, 192));
            }
        });

        _bus?.OnEvent((StopEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.MainButton != null)
            {
                group.MainButton.Call(p => p.Text = "Start");
                group.MainButton.Call(p => p.BackColor = Color.FromArgb(192, 255, 192));
            }
            if (group?.ProgressBar != null)
            {
                group.ProgressBar.Call(p => p.Value = 0);
            }
        });

        _bus?.OnEvent((StopRequestEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.MainButton != null)
            {
                group.MainButton.Call(p => p.Text = "Stopping");
                group.MainButton.Call(p => p.BackColor = Color.FromArgb(255, 255, 150));
            }
        });

        _bus?.OnEvent((StatusLabelChangeEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.MainButton != null)
            {
                group.StatusLabel?.Call(t => t.Text = e.Status);
                group.StatusLabel?.Call(t => t.Refresh());
            }
        });

        _bus?.OnEvent((AlertEvent e) =>
        {
            MessageBoxQueue.Add(e.Message);
        });

        _bus?.OnEvent((Error e) =>
        {
            handelErrorEvent(e);
        });
    }

    private ErrorModal _errodModel = new ErrorModal();
    private void handelErrorEvent(Error e)
    {
        var errorMessage = e.ToString();

        if (_errodModel.IsDisposed)
        {
            _errodModel = new ErrorModal();
        }

        if (_errodModel.Created)
        {
            _errodModel.AppendError(errorMessage);
        }
        else
        {
            _errodModel.ClearError();
            _errodModel.AppendError(errorMessage);
            _errodModel.Call(m => m.ShowDialog());
        }
    }

    public string GetVersion()
    {
        return _config.GetSection("version").Value ?? "could not get version";
    }
}
