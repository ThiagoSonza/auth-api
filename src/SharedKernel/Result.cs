using System.Diagnostics.CodeAnalysis;

namespace SharedKernel;

public class Result
{
    protected Result(bool isSuccess, IList<string> error = null!)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IList<string> Error { get; } = [];

    public static Result Success()
        => new(true);

    public static Result<TValue> Success<TValue>(TValue value)
        => Result<TValue>.Create(value, true);

    public static Result Failure(string error)
        => new(false, [error]);

    public static Result<TValue> Failure<TValue>(string error)
        => Result<TValue>.Create(default, false, [error]);

    public static Result Failure(List<string> errors)
        => new(false, errors);

    public static Result<TValue> Failure<TValue>(List<string> errors)
        => Result<TValue>.Create(default, false, errors);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(TValue? value, bool isSuccess, IList<string> errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value)
        => value is not null
            ? Success(value)
            : Failure<TValue>(["Error.NullValue"]);

    public static Result<TValue> ValidationFailure(IList<string> errors = null!)
        => new(default, false, errors);

    public static Result<TValue> Create(TValue? value, bool isSuccess, IList<string> errors = null!)
        => new(value, isSuccess, errors);
}