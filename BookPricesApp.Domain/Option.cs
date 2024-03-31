
namespace BookPricesApp.Core.Utils;

public readonly struct Result<TValue, TError>
{
    public readonly TValue? Value;
    public readonly TError? Error;

    private Result(TValue value)
    {
        Value = value;
        Error = default;
    }

    private Result(TError error)
    {
        Error = error;
        Value = default;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);
}