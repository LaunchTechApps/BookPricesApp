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
            new Dictionary<BookExchange, Panel>
            {
                { BookExchange.Amazon, amazon_pnl },
                { BookExchange.Ebay, ebay_pnl },
            },
            new Dictionary<BookExchange, Label>
            {
                { BookExchange.Amazon, amazon_tab },
                { BookExchange.Ebay, ebay_tab }
            },
             new List<SelectGroup>
             {
                 new SelectGroup
                 {
                     Exchange = BookExchange.Amazon,
                     SelectButton = amazon_main_btn,
                     SelectTextBox =  amazon_text_box,
                     ProgressBar = amazon_progress,
                     MainButton = amazon_main_btn
                 }
             }
        );

        _displayManager.SetActiveTab(BookExchange.Amazon);
    }

    private void BookPrices_Load(object sender, EventArgs e)
    {

    }

    private void amazon_tb_Click(object sender, EventArgs e)
    {
        _displayManager.SetActiveTab(BookExchange.Amazon);
    }

    private void ebay_tb_Click(object sender, EventArgs e)
    {
        _displayManager.SetActiveTab(BookExchange.Ebay);
    }

    private void amazon_filepicker_Click(object sender, EventArgs e)
    {
        _displayManager.FilePickerSelect(filePicker, BookExchange.Amazon);
    }

    private void amazon_btn_Click(object sender, EventArgs e)
    {
        _displayManager.Start(BookExchange.Amazon);
    }
}
