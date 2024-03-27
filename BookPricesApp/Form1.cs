using BookPricesApp;

namespace BookPrices;

public partial class BookPrices : Form
{
    private ViewModel _viewmodel;
    public BookPrices()
    {
        InitializeComponent();
        _viewmodel = new ViewModel(
            new Dictionary<BookExchange, Panel>
            {
                { BookExchange.Amazon, amazon_pnl },
                { BookExchange.Ebay, ebay_pnl },
            },
            new Dictionary<BookExchange, Label>
            {
                { BookExchange.Amazon, amazon_tab },
                { BookExchange.Ebay, ebay_tab }
            }
        );

        _viewmodel.SetActiveTab(BookExchange.Amazon);
    }

    private void BookPrices_Load(object sender, EventArgs e)
    {

    }

    private void amazon_tb_Click(object sender, EventArgs e)
    {
        _viewmodel.SetActiveTab(BookExchange.Amazon);
    }

    private void ebay_tb_Click(object sender, EventArgs e)
    {
        _viewmodel.SetActiveTab(BookExchange.Ebay);
    }
}
