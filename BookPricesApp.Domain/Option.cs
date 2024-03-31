using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Utils;

public class Option
{
    public Exception? Ex { get; set; }
    public Option() { }
    public Option(Exception ex) { Ex = ex; }
}
public class Option<T> : Option
{
    public T? Data { get; set; }
    public Option() { }
    public Option(Exception ex) { Ex = ex; }
}

public class OptionTask<T> : Task<T>
{
    public T? Value { get; }
    public Exception? Ex { get; }

    public OptionTask(T value) : base(() => value)
    {
        Value = value;
        Ex = null;
    }

    public OptionTask(Exception ex) : base(() =>
    {
        throw ex;
    })
    {
        Value = default;
        Ex = ex;
    }
}