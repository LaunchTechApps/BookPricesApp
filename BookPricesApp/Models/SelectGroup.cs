using BookPricesApp.Core.Domain.Types;

namespace BookPricesApp.GUI.Models;
public class SelectGroup
{
    public BookExchange Exchange { get; set; } = BookExchange.Unknown;
    public Button? SelectButton { get; set; }
    public Button? MainButton { get; set; }
    public Label? Tab { get; set; }
    public Panel? Panel { get; set; }
    public TextBox? SelectTextBox { get; set; }
    public ProgressBar? ProgressBar { get; set; }
    public Label? StatusLabel { get; set; }
}
