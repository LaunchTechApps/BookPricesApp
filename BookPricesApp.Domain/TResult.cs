
namespace BookPricesApp.Core.Utils;

public class TVoid { }

public readonly struct TResult
{
    public static TVoid Void => new TVoid();
}

public sealed record Error(string Message, string? Description = null)
{
    public static Error Create(string Message, string? Description = null) =>
        new Error(Message, Description);
    public static Error NotImplemented()
    {
        try { throw new NotImplementedException(); }
        catch (Exception ex) { return From(ex); }
    }

    public static Error From(Exception ex) => new(ex.Message, ex.StackTrace);

    public override string ToString() => $"Error: {Message} - {Description}";
}

public readonly struct TResult<TValue>
{
    private readonly TValue? _value = default;
    private readonly Error? _error = default;

    public bool DidError => _error != null;

    public Error Error => _error!;
    public TValue Value => _value!;

    public static TVoid Void => new TVoid();

    private TResult(Error error)
    {
        _error = error;
    }

    private TResult(TValue value)
    {
        _value = value;
    }

    public static implicit operator TResult<TValue>(TValue value) => new(value);
    public static implicit operator TResult<TValue>(Error ex) => new(ex);
}