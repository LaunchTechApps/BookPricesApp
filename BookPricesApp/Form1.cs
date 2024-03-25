namespace BookPrices;

public partial class BookPrices : Form
{
    List<Panel> _panelList = new List<Panel>();
    int index = 0;
    public BookPrices()
    {
        InitializeComponent();
        _panelList.AddRange(new List<Panel>() { panel1 });
        _panelList[0].BringToFront();
    }

    private void BookPrices_Load(object sender, EventArgs e)
    {

    }
}
