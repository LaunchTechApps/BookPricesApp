using Microsoft.Extensions.DependencyInjection;
using BookPricesApp.GUI.Models;
using BookPricesApp.GUI.Utils;
using BookPricesApp.GUI.Validators;
using BookPricesApp.Core.Domain.Types;
using BookPricesApp.Core.Access.Config;
using BookPricesApp.Core.Manager.App;
using BookPricesApp.Core.Engine;
using BookPricesApp.Core.Access.Amazon;
using BookPricesApp.Core.Domain.Events;
using BookPricesApp.Core.Utils;

namespace BookPricesApp.GUI;
public class ViewModel
{
    private List<SelectGroup> _selectGroup = new();
    private ServiceProvider _serviceProvider;
    private BookExchange _activeExchange = BookExchange.Amazon;
    public ViewModel(List<SelectGroup> selectGroup)
    {
        _selectGroup = selectGroup;

        var services = new ServiceCollection();
        services.AddSingleton<IConfigAccess, ConfigAccess>();
        services.AddSingleton<IAppManager, AppManager>();
        services.AddSingleton<AmazonEngine>();
        services.AddSingleton<IAmazonAccess, AmazonAccess>();
        services.AddSingleton<EventBus>();

        _serviceProvider = services.BuildServiceProvider();

        setupEventBusListener();
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
        // make sure to save the file path to config json file
        if (group == null || group.SelectTextBox == null)
        {
            MessageBox.Show("unable to find " + _activeExchange.ToString());
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
        string filePath = string.Empty;
        if (group.SelectTextBox != null && string.IsNullOrEmpty(group.SelectTextBox.Text))
        {
            MessageBox.Show("No file selected for " + group.Exchange.ToString());
            return;
        }
        else
        {
            filePath = group.SelectTextBox?.Text.Trim() ?? "";
        }

        if (!File.Exists(filePath))
        {
            MessageBox.Show("File not found: " + filePath);
            return;
        }

        var isbnArray = File.ReadAllLines(filePath)
            .Select(s => s.Trim())
            .ToArray();

        var firstISBN = isbnArray.FirstOrDefault() ?? "";
        if (!ISBNValidator.IsValidISBN(firstISBN))
        {
            MessageBox.Show("Fisrt line was an invalid ISBN: " + firstISBN);
            return;
        }

        _serviceProvider.GetService<IAppManager>()?.SubmitMainEvent(_activeExchange);
    }

    private void setupEventBusListener()
    {
        var bus = _serviceProvider.GetService<EventBus>();

        bus?.OnEvent((ProgressEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.ProgressBar != null)
            {
                group.ProgressBar.Invoke(p => p.Value = e.Count);
            }
        });

        bus?.OnEvent((StartEvent e) =>
        {
            var group = _selectGroup.FirstOrDefault(g => g.Exchange == e.Exchange);
            if (group?.MainButton != null)
            {
                group.MainButton.Invoke(p => p.Text = "Stop");
                group.MainButton.Invoke(p => p.BackColor = Color.FromArgb(255, 192, 192));
            }
        });

        bus?.OnEvent((StopEvent e) =>
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
    }
}