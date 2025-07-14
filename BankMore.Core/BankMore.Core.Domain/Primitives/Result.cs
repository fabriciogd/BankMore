namespace BankMore.Core.Domain.Primitives;

public class Result<TValue>
{
    public bool IsSuccess { get; }

    public TValue? Value { get; }

    public Error? Error { get; }

    private Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    public static implicit operator Result<TValue>(Error error) =>
        new(error);

    public static implicit operator Result<TValue>(TValue value) =>
        new(value);

    public static Result<TValue> Failure(Error error) =>
        new(error);

    public static Result<TValue> Success(TValue value) =>
    new(value);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure) =>
        IsSuccess ? success(Value) : failure(Error);

}
