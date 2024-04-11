using BookPricesApp.Core.Domain.Types;
using BookPricesApp.GUI;
using BookPricesApp.GUI.Models;
using BookPricesApp.GUI.Utils;

namespace BookPrices;
// TODO: No more exporting - delete the MiniExcel library
// TODO: Remove all Exchange Data from the Output table that we are running instead of dropping the table.
// TOOD: Create an amazon table that replaces the output table - each exchange will get its own table
public partial class MainForm : Form
{
    private ViewModel _vm;
    public MainForm()
    {
        InitializeComponent();

        _vm = new ViewModel(
             new List<SelectGroup>
             {
                 new SelectGroup
                 {
                     Exchange = BookExchange.Amazon,
                     SelectButton = amazon_select_btn,
                     SelectTextBox =  amazon_text_box,
                     ProgressBar = amazon_progress,
                     MainButton = amazon_main_btn,
                     Panel = amazon_pnl,
                     Tab = amazon_tab,
                     StatusLabel = amazon_status_lbl
                 },
                 new SelectGroup
                 {
                     Exchange = BookExchange.Ebay,
                     SelectButton = ebay_select_btn,
                     SelectTextBox =  ebay_text_box,
                     ProgressBar = ebay_progress,
                     MainButton = ebay_main_btn,
                     Panel = ebay_pnl,
                     Tab = ebay_tab,
                 }
             }
        );

        version_lbl.Text = "0.0.0";
        version_lbl.Refresh();

        _vm.SetActiveTab(BookExchange.Amazon);
    }

    private void BookPrices_Load(object sender, EventArgs e) { }


    private void amazon_main_btn_Click(object sender, EventArgs e)
    {
        _vm.SubmitMainButton();
    }
    private void amazon_select_btn_Click(object sender, EventArgs e)
    {
        _vm.FilePickerSelect(filePicker);
    }
    private void amazon_tab_Click(object sender, EventArgs e)
    {
        _vm.SetActiveTab(BookExchange.Amazon);
    }


    private void ebay_main_btn_Click(object sender, EventArgs e)
    {
        MessageBoxQueue.Add("Coming soon!");
        //_displayManager.SubmitMainButton();
    }
    private void ebay_select_btn_Click(object sender, EventArgs e)
    {
        _vm.FilePickerSelect(filePicker);
    }
    private void ebay_tab_Click(object sender, EventArgs e)
    {
        _vm.SetActiveTab(BookExchange.Ebay);
    }
}