namespace BookPrices;

public partial class BookPrices : Form
{
    private List<Panel> _panelList = new();
    private List<Label> _tabList = new();

    int index = 0;
    public BookPrices()
    {
        InitializeComponent();
        _panelList.AddRange(new List<Panel>() { amazon_pnl, ebay_pnl });
        _tabList.AddRange(new List<Label>() { amazon_tb, ebay_tb });
        _panelList[0].BringToFront();
    }

    private void BookPrices_Load(object sender, EventArgs e)
    {

    }

    private void amazon_tb_Click(object sender, EventArgs e)
    {
        _tabList.ForEach(t => t.BackColor = Color.FromName("Control"));
        _tabList[0].BackColor = Color.FromName("ActiveBorder");
        _panelList[0].BringToFront();
    }

    private void ebay_tb_Click(object sender, EventArgs e)
    {
        _tabList.ForEach(t => t.BackColor = Color.FromName("Control"));
        _tabList[1].BackColor = Color.FromName("ActiveBorder");
        _panelList[1].BringToFront();
    }
}
