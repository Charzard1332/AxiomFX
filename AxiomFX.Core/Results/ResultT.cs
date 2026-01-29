namespace AxiomFX.Core.Results;

/// <summary>
/// Represents the outcome of an operation that may succeed or fail,
/// and optionally returns a value when successful.
/// </summary>
/// <typeparam name="T">The type of the value returned on success.</typeparam>
public readonly struct Result<T>
{
    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// The value returned when the operation succeeds.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Contains error information when the operation fails.
    /// </summary>
    public Error? Error { get; }

    private Result(bool success, T? value, Error? error)
    {
        Success = success;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    public static Result<T> Ok(T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return new Result<T>(true, value, null);
    }

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    public static Result<T> Fail(Error error)
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error));

        return new Result<T>(false, default, error);
    }

    /// <summary>
    /// Implicit conversion from a value to a successful result.
    /// </summary>
    public static implicit operator Result<T>(T value) => Ok(value);

    /// <summary>
    /// Implicit conversion from an error to a failed result.
    /// </summary>
    public static implicit operator Result<T>(Error error) => Fail(error);

    /// <summary>
    /// Throws the contained error as an exception if the result is a failure.
    /// Useful for bridging imperative and result-based code.
    /// </summary>
    public T Unwrap()
    {
        if (!Success)
            throw Error!.Exception ?? new InvalidOperationException(Error.ToString());

        return Value!;
    }

    /// <summary>
    /// Returns the value if successful, otherwise returns the provided fallback.
    /// </summary>
    public T Or(T fallback) => Success ? Value! : fallback;

    /// <summary>
    /// Returns the value if successful, otherwise invokes the fallback factory.
    /// </summary>
    public T Or(Func<Error, T> fallbackFactory)
    {
        if (fallbackFactory is null)
            throw new ArgumentNullException(nameof(fallbackFactory));

        return Success ? Value! : fallbackFactory(Error!);
    }

    public override string ToString()
        => Success ? $"Success: {Value}" : $"Failure: {Error}";
}