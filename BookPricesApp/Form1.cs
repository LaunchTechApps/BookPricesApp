using BookPricesApp;
using BookPricesApp.Domain.Types;
using BookPricesApp.Manager.Display;
using BookPricesApp.Manager.Display.Models;

namespace BookPrices;

public partial class BookPrices : Form
{
    private DisplayManager _displayManager;
    public BookPrices()
    {
        InitializeComponent();

        _displayManager = new DisplayManager(
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

        _displayManager.SetActiveTab(BookExchange.Amazon);
    }

    private void BookPrices_Load(object sender, EventArgs e) { }


    private void amazon_main_btn_Click(object sender, EventArgs e)
    {
        _displayManager.SubmitMainButton();
    }
    private void amazon_select_btn_Click(object sender, EventArgs e)
    {
        _displayManager.FilePickerSelect(filePicker);
    }
    private void amazon_tab_Click(object sender, EventArgs e)
    {
        _displayManager.SetActiveTab(BookExchange.Amazon);
    }


    private void ebay_main_btn_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Coming soon!");
        //_displayManager.SubmitMainButton();
    }
    private void ebay_select_btn_Click(object sender, EventArgs e)
    {
        _displayManager.FilePickerSelect(filePicker);
    }
    private void ebay_tab_Click(object sender, EventArgs e)
    {
        _displayManager.SetActiveTab(BookExchange.Ebay);
    }
}
