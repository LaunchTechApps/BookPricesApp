using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Engine.Amazon;

public class AmazonEngineConfig
{
    public ProgressBar? ProgressBar { get; set; }
    public string? BaseUrl { get; set; }
    public Button? Button { get; set; }
}
public class AmazonEngine
{
    private bool _running = false;
    public AmazonEngine()
    {

    }

    public void Run()
    {
        if (_running)
        {
            MessageBox.Show("Amazon is already running");
            return;
        }
        _running = true;
    }

    public void Stop()
    {
        if (!_running)
        {
            MessageBox.Show("Amazon is NOT running");
            return;
        }
        _running = false;
    }
}
