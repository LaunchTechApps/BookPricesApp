using Microsoft.Extensions.DependencyInjection;
using BookPricesApp.GUI.Models;
using BookPricesApp.GUI.Utils;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Manager.App;
using BookPricesApp.Core.Engine;
using BookPricesApp.Core.Access.Amazon;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Utils;
using BookPricesApp.Core.Domain.Config;
using BookPricesApp.Core.Access.FlatFile;

namespace BookPricesApp.GUI;
public class ViewModel
{
    private List<SelectGroup> _selectGroup = new();
    private ServiceProvider _serviceProvider;
    private BookExchange _activeExchange = BookExchange.Amazon;
    private EventBus _bus;
    private AppConfig _appConfig;
    public ViewModel(List<SelectGroup> selectGroup)
    {
        _selectGroup = selectGroup;

        _bus = new EventBus();
        var flatFileAccess = new FlatFileAccess(_bus);
        _appConfig = flatFileAccess.GetAppConfig().Data!;

        var services = new ServiceCollection();

        services.AddSingleton(_bus);
        services.AddSingleton<IFlatFileAccess>(flatFileAccess);
        services.AddSingleton(_appConfig);
        services.AddSingleton<IAppManager, AppManager>();
        services.AddSingleton<AmazonEngine>();
        services.AddSingleton<EngineProvider>();
        services.AddSingleton<AmazonAccess>();

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
        // TODO: this should be done in the manager and the display manager should just have to show message boxes
        if (group.SelectTextBox != null && string.IsNullOrEmpty(group.SelectTextBox.Text))
        {
            MessageBoxQueue.Add($"No file selected for {group.Exchange}");
            return;
        }
        else
        {
            isbnFilePath = group.SelectTextBox?.Text.Trim() ?? "";
        }

        _serviceProvider.GetService<IAppManager>()?.SubmitMainEvent(_activeExchange, isbnFilePath);
    }

    private void setupSavedFilePaths()
    {
        var amazonGroup = _selectGroup.FirstOrDefault(g => g.Exchange == BookExchange.Amazon);
        if (amazonGroup != null)
        {
            if (amazonGroup.SelectTextBox != null && !string.IsNullOrEmpty(_appConfig.Amazon.IsbnFilePath))
            {
                amazonGroup.SelectTextBox.Text = _appConfig.Amazon.IsbnFilePath;
                amazonGroup.SelectTextBox.Refresh();
            }
            return;
        }

        var ebayGroup = _selectGroup.FirstOrDefault(g => g.Exchange == BookExchange.Ebay);
        if (ebayGroup != null)
        {
            if (ebayGroup.SelectTextBox != null && !string.IsNullOrEmpty(_appConfig.Ebay.IsbnFilePath))
            {
                ebayGroup.SelectTextBox.Text = _appConfig.Amazon.IsbnFilePath;
                ebayGroup.SelectTextBox.Refresh();
            }
            return;
        }
    }

    private void setupEventBus()
    {
        _bus?.OnEvent((ProgressEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.ProgressBar != null)
            {
                group.ProgressBar.Invoke(p => p.Value = e.Count);
            }
        });

        _bus?.OnEvent((StartEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.MainButton != null)
            {
                group.MainButton.Invoke(p => p.Text = "Stop");
                group.MainButton.Invoke(p => p.BackColor = Color.FromArgb(255, 192, 192));
            }
        });

        _bus?.OnEvent((StopEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.MainButton != null)
            {
                group.MainButton.Invoke(p => p.Text = "Start");
                group.MainButton.Invoke(p => p.BackColor = Color.FromArgb(192, 255, 192));
            }
            if (group?.ProgressBar != null)
            {
                group.ProgressBar.Invoke(p => p.Value = 0);
            }
        });

        _bus?.OnEvent((StopRequestEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.MainButton != null)
            {
                group.MainButton.Invoke(p => p.Text = "Stopping");
                group.MainButton.Invoke(p => p.BackColor = Color.FromArgb(255, 255, 150));
            }
        });

        _bus?.OnEvent((AlertEvent e) =>
        {
            MessageBoxQueue.Add(e.Message);
        });
    }
}
