using BookPricesApp.Core.Domain;
using BookPricesApp.GUI.Utils;

namespace BookPricesApp.GUI;
public partial class ErrorModal : Form
{
    public string Error { get; set; } = string.Empty;
    public ErrorModal()
    {
        InitializeComponent();
    }

    public void AppendError(string error)
    {
        var time = DateTime.Now.ToIso8601();

        if (string.IsNullOrEmpty(error_rtb.Text))
        {
            error_rtb.Call(rtb => rtb.Text = $"{time}\n{error}");
        }
        else
        {
            error_rtb.Call(rtb => rtb.Text = $"{error_rtb.Text}\n\n{time}\n{error}");
        }

        error_rtb.Call(rtb => rtb.Refresh());
    }

    public void ClearError()
    {
        error_rtb.Call(rtb => rtb.Clear());
        error_rtb.Call(rtb => rtb.Refresh());
    }
}
