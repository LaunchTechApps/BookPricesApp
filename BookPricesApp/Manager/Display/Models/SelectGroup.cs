using BookPricesApp.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Manager.Display.Models;
public class SelectGroup
{
    public BookExchange? Exchange { get; set; }
    public Button? SelectButton { get; set; }
    public Button? MainButton { get; set; }
    public Label? Tab { get; set; }
    public Panel? Panel { get; set; }
    public TextBox? SelectTextBox { get; set; }
    public ProgressBar? ProgressBar { get; set; }
}
